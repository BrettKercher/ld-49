using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmotionComponent : MonoBehaviour {
    public SpriteRenderer Bubble;

    public SpriteRenderer Icon;

    public float FlashHoldTime = 0.5f;

    private Dictionary<Emote, Sprite> _emoteDict;
    private GameManager _gameManager;

    private Sequence _activeSequence;
    private Emote _currentEmote;

    public enum Emote {
        NONE = 0,
        ALERTED = 1,
        CURIOUS = 2,
        ANGRY = 3,
    }

    public void ShowEmote(Emote emote) {
        if (_activeSequence != null && _currentEmote != emote) {
            _activeSequence.Kill();
            FadeOut(0.1f).OnComplete(() => {
                DoShowEmote(emote);
            });
        }
        else if (_currentEmote != emote) {
            DoShowEmote(emote);
        }
    }
    
    public void HideEmote(Emote emote) {
        if (_currentEmote != emote) {
            return;
        }

        if (_activeSequence != null) {
            _activeSequence.Kill();
            _activeSequence = null;
            _currentEmote = Emote.NONE;
        }
        FadeOut();
    }
    
    public void FlashEmote(Emote emote) {
        if (_activeSequence != null) {
            _activeSequence.Kill();
            FadeOut(0.1f).OnComplete(() => {
                DoFlashEmote(emote);
            });
        }
        else {
            DoFlashEmote(emote);
        }
    }
    
    void Awake() {
        FadeOut();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void DoShowEmote(Emote emote) {
        Icon.sprite = _gameManager.GetEmoteSprite(emote);
        _currentEmote = emote;
        _activeSequence = FadeIn();
    }

    private void DoFlashEmote(Emote emote) {
        Icon.sprite = _gameManager.GetEmoteSprite(emote);
        _currentEmote = emote;
        _activeSequence = CreateFlashSequence();
        _activeSequence.OnComplete(() => {
            _activeSequence = null;
            _currentEmote = Emote.NONE;
        });
    }

    private Sequence CreateFlashSequence() {
        var fadeInSequence = FadeIn();
        var fadeOutSequence = FadeOut();

        var seq = DOTween.Sequence();
        seq.Insert(0, fadeInSequence)
            .Insert(fadeInSequence.Duration() + FlashHoldTime, fadeOutSequence);

        return seq;
    }
    
    private Sequence FadeOut(float time = 0.5f) {
        var sequence = DOTween.Sequence();
        sequence.Append(Icon.DOFade(0, time))
            .Insert(0, Bubble.DOFade(0, time));
        return sequence;
    }
    
    private Sequence FadeIn(float time = 0.5f) {
        var sequence = DOTween.Sequence();
        sequence.Append(Bubble.DOFade(1, time))
            .Insert(0, Icon.DOFade(1, time));
        return sequence;
    }
}
