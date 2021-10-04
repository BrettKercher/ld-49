using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Winner : MonoBehaviour {
    [SerializeField] private Text _text;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Text _buttonText;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _delayTime;

    public IEnumerator Trigger() {
        yield return new WaitForSeconds(_delayTime);

        var sequence = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();
        var sequence3 = DOTween.Sequence();
        sequence.Append(_text.DOFade(1, _fadeTime));
        sequence2.Append(_buttonImage.DOFade( 1, _fadeTime));
        sequence3.Append(_buttonText.DOFade(1, _fadeTime));
        
    }
}
