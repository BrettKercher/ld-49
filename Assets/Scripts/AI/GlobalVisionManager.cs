using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;


[BurstCompile]
internal struct FilterNodesByDistanceAndAngleJob : IJobParallelForFilter {
    [ReadOnly] public float maxDistance;
    [ReadOnly] public float3 targetPosition;
    [ReadOnly] public float maxHalfAngle;
    [ReadOnly] public float3 forward;
    [ReadOnly] public NativeArray<float3> nodePositions;
    public NativeArray<float> distances;
 
    public bool Execute(int index) {
        var distance = math.distance(targetPosition, nodePositions[index]);
        var direction = nodePositions[index] - targetPosition;
        var angle = Vector3.Angle(direction, forward);
        distances[index] = distance;
        return (distance <= maxDistance) && (angle <= maxHalfAngle);
    }
}

[BurstCompile]
internal struct BuildContainerFromFilteredIndicesJob<T> : IJobParallelFor where T : struct {
    [NativeDisableParallelForRestriction][ReadOnly] public NativeArray<T> UnfilteredObjects;
    [NativeDisableParallelForRestriction] [ReadOnly] public NativeArray<int> FilteredIndices;
    public NativeArray<T> FilteredObjects;

    public void Execute(int index) {
        FilteredObjects[index] = UnfilteredObjects[FilteredIndices[index]];
    }
}

// Keeps track of which entities can see which nodes
public class GlobalVisionManager : MonoBehaviour {

    [SerializeField] private VisualEffectsTileMap _effectsTileMap = null;
    [SerializeField] private TileBase _highlightTile = null;
    [SerializeField] private Color _visionRangeColor = Color.red;

    private HashSet<Vector3> _visibleNodes = new HashSet<Vector3>();
    private HashSet<Vector3> _previouslyVisibleNodes = new HashSet<Vector3>();
    
    private GridGraph _targetingGraph;

    public void UpdateVisionList(Vector3[] visibleNodes) {
        foreach (var node in visibleNodes) {
            _visibleNodes.Add(node);
        }
    }

    public Vector3[] CalculateVisibleNodesInRange(float range, Vector3 startingPosition, float viewAngle, Vector3 forwardVector) {
        var nodes = _targetingGraph.nodes.Where(x => x != null && x.Walkable);
        var nodePositions = nodes.Select(n => (float3)((Vector3)n.position)).ToArray();

        // Filter node positions by distance. This job returns an array of indices which are used in the next job to
        // build an array of the filtered positions
        var allNodePositions = new NativeArray<float3>(nodePositions, Allocator.TempJob);
        var allNodeDistances = new NativeArray<float>(nodePositions.Length, Allocator.TempJob);
        var filteredByDistanceIndices = new NativeList<int>(Allocator.TempJob);
        var distanceJob = new FilterNodesByDistanceAndAngleJob {
            maxDistance = range,
            targetPosition = startingPosition, 
            maxHalfAngle = viewAngle / 2,
            forward = forwardVector,
            nodePositions = allNodePositions,
            distances = allNodeDistances, // <-- gets written to
        };
        var distanceHandle = distanceJob.ScheduleAppend(filteredByDistanceIndices, nodePositions.Length, 32);
        distanceHandle.Complete();

        // Use the list of indices from the previous job to build an array containing only the nodes whose distance from target is < range
        var filteredByDistancePositions = new NativeArray<float3>(filteredByDistanceIndices.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        var buildPositionsHandle = new BuildContainerFromFilteredIndicesJob<float3> {
            UnfilteredObjects = allNodePositions,
            FilteredIndices = filteredByDistanceIndices,
            FilteredObjects = filteredByDistancePositions, // <-- gets written to
        }.Schedule(filteredByDistanceIndices.Length, 32, distanceHandle);
        buildPositionsHandle.Complete();

        // Use the list of filtered indices to build an array of distances for the remaining nodes.
        // These distances are used later in the raycast job to determine line of sight
        var filteredDistances = new NativeArray<float>(filteredByDistanceIndices.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        var buildDistancesHandle = new BuildContainerFromFilteredIndicesJob<float> {
            UnfilteredObjects = allNodeDistances,
            FilteredIndices = filteredByDistanceIndices,
            FilteredObjects = filteredDistances, // <-- gets written to
        }.Schedule(filteredByDistanceIndices.Length, 32, distanceHandle);
        buildDistancesHandle.Complete();


        var filteredByLOSPositions = new List<Vector3>();
        for (var i = 0; i < filteredByDistancePositions.Length; i++) {
            var nodePos = filteredByDistancePositions[i];
            var nodePos2 = new float2(nodePos.x, nodePos.y);
            var direction3 = (float3) startingPosition - nodePos;
            var direction2 = new float2(direction3.x, direction3.y);
            if (!Physics2D.Raycast(nodePos2, direction2, filteredDistances[i],
                LayerMask.GetMask("Ground"))) {
                filteredByLOSPositions.Add(nodePos);
            }
        }

        // Dispose all of our NativeArrays
        filteredByDistanceIndices.Dispose();
        allNodePositions.Dispose();
        allNodeDistances.Dispose();
        filteredByDistancePositions.Dispose();
        filteredDistances.Dispose();

        return filteredByLOSPositions.ToArray();
    }

    private void Awake() {
        _targetingGraph = (GridGraph) AstarPath.active.data.FindGraph(g => g.graphIndex == 1);
    }

    private void LateUpdate() {
        var newNodes = _visibleNodes.Except(_previouslyVisibleNodes).ToArray();
        var oldNodes = _previouslyVisibleNodes.Except(_visibleNodes).ToArray();
        
        _effectsTileMap.HighlightNodes(newNodes, _highlightTile);
        _effectsTileMap.RemoveHighlightFromNodes(oldNodes);
        
        _previouslyVisibleNodes = _visibleNodes;
        _visibleNodes = new HashSet<Vector3>();
    }

    private void Start() {
        _effectsTileMap.SetTilemapColor(_visionRangeColor);
    }
}