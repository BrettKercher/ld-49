using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmotionComponent : MonoBehaviour {
    private GameManager _gameManager;

    [SerializeField] private GameObject _emoteBubblePrefab;
    private EmoteBubble _currentEmote;

    public enum Emote {
        NONE = 0,
        ALERTED = 1,
        CURIOUS = 2,
        ANGRY = 3,
    }

    public void ShowEmote(Emote emote) {
        if (_currentEmote != null) {
            Destroy(_currentEmote);
            return;
        }

        var sprite = _gameManager.GetEmoteSprite(emote);
        var emoteBubble = Instantiate(_emoteBubblePrefab).GetComponent<EmoteBubble>();
        emoteBubble.Init(sprite);
        _currentEmote = emoteBubble;
    }
    
    public void HideEmote(Emote emote) {
        if (_currentEmote == null) {
            return;
        }
        _currentEmote.Kill();
    }
    
    
    void Awake() {
        _gameManager = FindObjectOfType<GameManager>();
    }
}
