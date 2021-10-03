using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {
    private GameManager _gameManager;
    private EmotionComponent _emote;

    private void Awake() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _emote = GetComponent<EmotionComponent>();
    }

    private void Start() {
        _gameManager.SubscribeToHorseSpottedEvent(OnHorseSpotted);
    }

    private void OnHorseSpotted(GameObject spotter) {
        Debug.Log("Spotted a horse!");
        _emote.ShowEmote(EmotionComponent.Emote.ALERTED);
    }
}