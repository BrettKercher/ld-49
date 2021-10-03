using System;
using UnityEngine;
using System.Collections;
using Pathfinding;


[Serializable]
public struct PatrolTarget {
    public Transform position;
    public float delay;
}

[UniqueComponent(tag = "ai.destination")]
public class AdvancedPatrol : VersionedMonoBehaviour {

    public PatrolTarget[] targets;

    // index of current target
    private int _index;

    private IAstarAI _agent;
    private float _switchTime = float.PositiveInfinity;

    protected override void Awake () {
        base.Awake();
        _agent = GetComponent<IAstarAI>();
    }

    private void Update () {
        if (targets.Length == 0) return;

        var search = false;

        var currentTarget = targets[_index];

        // Note: using reachedEndOfPath and pathPending instead of reachedDestination here because
        // if the destination cannot be reached by the agent, we don't want it to get stuck, we just want it to get as close as possible and then move on.
        if (_agent.reachedEndOfPath && !_agent.pathPending && float.IsPositiveInfinity(_switchTime)) {
            _switchTime = Time.time + currentTarget.delay;
        }

        if (Time.time >= _switchTime) {
            _index++;
            search = true;
            _switchTime = float.PositiveInfinity;
        }

        _index %= targets.Length;
        _agent.destination = targets[_index].position.position;

        if (search) _agent.SearchPath();
    }
}