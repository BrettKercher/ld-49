using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour {

    [SerializeField] private Text _text;
    [SerializeField] private float _fadeDuration;

    private Sequence _activeSequence;

    public IEnumerator ShowText(string text, float holdTime) {
        if (_activeSequence != null) {
            yield return _activeSequence.WaitForCompletion();
        }

        _text.text = text;

        _activeSequence = DOTween.Sequence();
        _activeSequence.Insert(0, _text.DOFade(1, _fadeDuration));
        _activeSequence.Insert(_fadeDuration + holdTime, _text.DOFade(0, _fadeDuration));

        yield return _activeSequence.WaitForCompletion();

        _text.text = "";
    }
}