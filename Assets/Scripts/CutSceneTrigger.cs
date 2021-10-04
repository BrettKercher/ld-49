using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour {

    [SerializeField] private GameObject _panTarget;
    [SerializeField] private float _panSpeed;
    [SerializeField] private float _holdTime;

    [SerializeField] private StoryText _storyTextObject;
    [SerializeField] [TextArea(1,20)] private string _text;
    [SerializeField] private float _textDuration;

    private MainCamera _mainCamera;

    private void Awake() {
        _mainCamera = Camera.main.GetComponent<MainCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) {
            return;
        }
        
        _mainCamera.SubscribeToArrivedEvents(OnPanToTargetFinished, OnPanBackFinished);
        _mainCamera.PanToPositionAndBack(_panTarget.transform.position, other.gameObject, _holdTime, _panSpeed);
    }

    private void OnPanToTargetFinished(MainCamera dispatcher) {
        StartCoroutine(_storyTextObject.ShowText(_text, _textDuration));
    }

    private void OnPanBackFinished(MainCamera dispatcher) {
        dispatcher.UnsubscribeFromArrivedEvents(OnPanToTargetFinished, OnPanBackFinished);
        Destroy(gameObject);
    }
}