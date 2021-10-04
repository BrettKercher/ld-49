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

    [SerializeField] private bool _playInReverse;
    public PatrolTarget[] targets;

    // index of current target
    private int _index;

    private AIPath _agent;
    private Coroutine _currentAction;

    private bool _reversing;

    protected override void Awake () {
        base.Awake();
        _agent = GetComponent<AIPath>();
    }

    private void OnPatrolActionComplete(PatrolArriveAction dispatcher) {
        if (dispatcher != null) {
            dispatcher.UnsubscribeFromCompleteEvent(OnPatrolActionComplete);
        }
        _currentAction = null;
        _agent.rotation = transform.rotation;
        _agent.rotationSpeed = 360;
        
        _index += _reversing ? -1 : 1;
        if (_playInReverse) {
            if (_reversing && _index == 0) {
                _reversing = false;
            } 
            else if (!_reversing && _index == targets.Length - 1) {
                _reversing = true;
            }
        }
        else {
            _index %= targets.Length;
        }

        _agent.destination = targets[_index].position.position;
        _agent.SearchPath();
    }

    private void Update () {
        if (targets.Length == 0 || _currentAction != null) return;

        var currentTarget = targets[_index];

        if (_agent.reachedEndOfPath && !_agent.pathPending) {
            if (_agent.reachedDestination && currentTarget.arriveAction != null) {
                // if we did reach the destination, wait for the action to complete, then move to next target
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