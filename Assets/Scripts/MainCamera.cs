using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private GameObject _target;

    private Vector3 _panTarget;
    private Vector3 _velocity = Vector3.zero;
    private bool _trackingTarget;

    private bool _panningThere;
    private bool _panningBack;
    private float _panToTargetHoldTime;
    private Vector3 _returnPosition;
    private bool _autoPanning;
    private bool _shouldPanBack;
    
    public delegate void BackToOldTargetHandler(MainCamera dispatcher);
    private event BackToOldTargetHandler BackToOldTarget;
    
    public delegate void AtNewTargetHandler(MainCamera dispatcher);
    private event AtNewTargetHandler AtNewTarget;

    public void SetTrackedTarget(GameObject target) {
        _target = target;
        _trackingTarget = true;
    }
    
    public void PanToPositionAndBack(Vector3 target, Vector3 returnPosition, float holdDuration) {
        _autoPanning = true;
        _trackingTarget = false;
        _panTarget = target;
        _returnPosition = returnPosition;

        _panningThere = true;
        _panningBack = false;
        _panToTargetHoldTime = holdDuration;
        _shouldPanBack = true;
    }
    
    public void PanToPosition(Vector3 target) {
        _autoPanning = true;
        _trackingTarget = false;
        _panTarget = target;

        _panningThere = true;
        _panningBack = false;
        _shouldPanBack = false;
    }
    
    public void SubscribeToArrivedEvents(AtNewTargetHandler newTargetFunction, BackToOldTargetHandler oldTargetFunction) {
        AtNewTarget += newTargetFunction;
        BackToOldTarget += oldTargetFunction;
    }

    public void UnsubscribeFromArrivedEvents(AtNewTargetHandler newTargetFunction, BackToOldTargetHandler oldTargetFunction) {
        AtNewTarget -= newTargetFunction;
        BackToOldTarget -= oldTargetFunction;
    }
    
    private void Update() {
        if (_autoPanning) {
            return;
        }

        if (_trackingTarget) {
            _panTarget = _target.transform.position;
        }
    }

    private void FixedUpdate() {
        var goalPos = _trackingTarget ? _target.transform.position : _panTarget;
        var myPosition = transform.position;
        goalPos.z = -10f;
        transform.position = Vector3.SmoothDamp(myPosition, goalPos, ref _velocity, _smoothTime);

        if (_panningThere && (goalPos - transform.position).sqrMagnitude < 1) {
            _panningThere = false;
            AtNewTarget?.Invoke(this);
            if (_shouldPanBack) {
                StartCoroutine(SetTargetAfterDelay(_returnPosition, _panToTargetHoldTime));
            }
        }
        
        if (_panningBack && (goalPos - transform.position).magnitude < 1) {
            _panningBack = false;
            _autoPanning = false;
            BackToOldTarget?.Invoke(this);
        }
    }
    
    private IEnumerator SetTargetAfterDelay(Vector3 targetPosition, float delay) {
        yield return new WaitForSeconds(delay);
        _panTarget = targetPosition;
        _panningBack = true;
    }
}