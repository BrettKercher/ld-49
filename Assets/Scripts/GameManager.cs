using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    
    [Serializable]
    public struct EmoteSpriteMapping {
        public EmotionComponent.Emote emote;
        public Sprite sprite;
    }
    
    public EmoteSpriteMapping[] EmoteSpriteMappings;
    private Dictionary<EmotionComponent.Emote, Sprite> _emoteDict = new Dictionary<EmotionComponent.Emote, Sprite>();
    
    private MainCamera _mainCamera;

    public delegate void HorseSpottedHandler(GameObject spotter);
    public event HorseSpottedHandler HorseSpotted;

    public void SubscribeToHorseSpottedEvent(HorseSpottedHandler onSpottedFunction) {
        HorseSpotted += onSpottedFunction;
    }
    
    public void UnsubscribeToHorseSpottedEvent(HorseSpottedHandler onSpottedFunction) {
        HorseSpotted -= onSpottedFunction;
    }

    public void OnHorseSpotted(GameObject spotter) {
        _mainCamera.PanToPosition(spotter.transform.position);
        HorseSpotted?.Invoke(spotter);
    }

    public Sprite GetEmoteSprite(EmotionComponent.Emote emote) {
        return _emoteDict[emote];
    }

    private void Awake() {
        _mainCamera = Camera.main.GetComponent<MainCamera>();
    }

    private void Start() {
        foreach (var mapping in EmoteSpriteMappings) {
            _emoteDict.Add(mapping.emote, mapping.sprite);
        }
    }
}