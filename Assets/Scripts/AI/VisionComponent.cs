using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class VisionComponent : MonoBehaviour {
    [SerializeField] private float _visionRange = 12;
    [SerializeField] private float _visionAngle = 120f;

    private readonly Dictionary<int, GameObject> _awareEntities = new Dictionary<int, GameObject>();
    private GridGraph _targetingGraph;
    private VisionRangeVisualizer _visualizer;

    public float VisionRange => _visionRange;
    public float VisionAngle => _visionAngle;

    private bool _horseSpotted;
    private GameManager _gameManager;

    public GameObject[] AwareEntities() {
        return _awareEntities.Values.ToArray();
    }

    public void SetVisualizerEnable(bool value) {
        _visualizer.SetEnabled(value);
    }

    private void Awake() {
        _visualizer = GetComponent<VisionRangeVisualizer>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start() {
        _targetingGraph = (GridGraph) AstarPath.active.data.FindGraph(g => g.graphIndex == 1);
    }

    private void Update() {
        var closeEntities = Physics2D.OverlapCircleAll(transform.position, _visionRange + 0.25f, LayerMask.GetMask("SpotZones")).Select(i => i.gameObject);
        _awareEntities.Clear();

        foreach (var entity in closeEntities) {
            if (entity == null) {
                continue;
            }

            // Ignore ourselves
            if (entity == gameObject) {
                continue;
            }

            var activeNode = _targetingGraph.GetNearest(entity.transform.position).node;
            if (!IsNodeVisible(activeNode)) {
                continue;
            }

            if (!_horseSpotted) {
                _horseSpotted = true;
                _gameManager.OnHorseSpotted(gameObject);
            }

            _awareEntities[entity.GetInstanceID()] = entity;
        }
    }

    private bool IsNodeVisible(GraphNode node) {
        var nodePosition = (Vector3) node.position;
        var ourPosition = transform.position;

        //Distance Check
        var nodeDistance = Vector3.Distance(nodePosition, ourPosition);
        if (nodeDistance > VisionRange) {
            return false;
        }

        // Angle Check
        var direction = nodePosition - ourPosition;
        var forward = transform.up;
        var angle = Vector3.Angle(direction, forward);
        if (angle > VisionAngle / 2) {
            return false;
        }

        // Line Of Sight Check
        var ourPositionWithOffset = ourPosition;
        var nodePositionWithOffset = nodePosition;
        return !Physics2D.Raycast(ourPositionWithOffset, nodePositionWithOffset, LayerMask.GetMask("Ground"));
    }
}