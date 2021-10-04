using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Winner : MonoBehaviour {
    [SerializeField] private Text _text;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _delayTime;

    public IEnumerator Trigger() {
        yield return new WaitForSeconds(_delayTime);

        var sequence = DOTween.Sequence();
        sequence.Append(_text.DOFade(1, _fadeTime));
    }
}
