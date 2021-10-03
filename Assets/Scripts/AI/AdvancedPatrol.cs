using System;
using UnityEngine;
using System.Collections;
using Pathfinding;


[Serializable]
public struct PatrolTarget {
    public Transform position;
    public float delay;
    public PatrolArriveAction arriveAction;
}

[UniqueComponent(tag = "ai.destination")]
public class AdvancedPatrol : VersionedMonoBehaviour {

    public PatrolTarget[] targets;

    // index of current target
    private int _index;

    private AIPath _agent;
    private Coroutine _currentAction;

    protected override void Awake () {
        base.Awake();
        _agent = GetComponent<AIPath>();
    }

    private void OnPatrolActionComplete(PatrolArriveAction dispatcher) {
        dispatcher.UnsubscribeFromCompleteEvent(OnPatrolActionComplete);
        _currentAction = null;
        _agent.isStopped = false;
        _agent.rotation = transform.rotation;
        _agent.rotationSpeed = 360;

        _index++;
        _index %= targets.Length;

        _agent.destination = targets[_index].position.position;
        _agent.SearchPath();
    }

    private void Update () {
        if (targets.Length == 0 || _currentAction != null) return;

        var currentTarget = targets[_index];

        if (_agent.reachedEndOfPath && !_agent.pathPending) {
            if (_agent.reachedDestination) {
                // if we did reach the destination, wait for the action to complete, then move to next target
                _agent.isStopped = true;
                _agent.rotationSpeed = 0;
                currentTarget.arriveAction.SubscribeToCompleteEvent(OnPatrolActionComplete);
                _currentAction = StartCoroutine(currentTarget.arriveAction.Execute(gameObject));
                return;
            }
            else {
                // if we did not reach the destination, skip the action and move to the next target
                OnPatrolActionComplete(null);
                return;
            }
        }
        _agent.destination = currentTarget.position.position;
    }
}