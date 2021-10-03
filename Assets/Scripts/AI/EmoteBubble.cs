using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmoteBubble : MonoBehaviour {

    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private SpriteRenderer _bubble;

    public void Init(Sprite icon) {
        _icon.sprite = icon;
        FadeIn();
    }

    public void Kill() {
        FadeOut();
    }

    private Sequence FadeIn(float time = 0.5f) {
        var sequence = DOTween.Sequence();
        sequence.Append(_bubble.DOFade(1, time))
            .Insert(0, _icon.DOFade(1, time));
        return sequence;
    }
    
    private Sequence FadeOut(float time = 0.5f) {
        var sequence = DOTween.Sequence();
        sequence.Append(_icon.DOFade(0, time))
            .Insert(0, _bubble.DOFade(0, time));
        return sequence;
    }
}