using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Guard : MonoBehaviour {
    private GameManager _gameManager;
    private EmotionComponent _emote;
    private AIPath _pathFinding;
    private VisionComponent _visionComponent;

    private void Awake() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _emote = GetComponent<EmotionComponent>();
        _pathFinding = GetComponent<AIPath>();
        _visionComponent = GetComponent<VisionComponent>();
    }

    private void Start() {
        _gameManager.SubscribeToHorseSpottedEvent(OnHorseSpotted);
    }

    private void OnHorseSpotted(GameObject spotter) {
        if (spotter == gameObject) {
            _emote.ShowEmote(EmotionComponent.Emote.ALERTED);
        }

        _pathFinding.isStopped = true;
        _visionComponent.SetVisualizerEnable(false);
    }

    private void OnDestroy() {
        _gameManager.UnsubscribeToHorseSpottedEvent(OnHorseSpotted);
    }
}