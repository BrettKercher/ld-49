using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    [SerializeField] private Text _text;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _delayTime;

    private GameManager _gameManager;

    public void Awake() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameManager.SubscribeToHorseSpottedEvent(OnHoseSpotted);
    }

    private void OnHoseSpotted(GameObject spotter) {
        StartCoroutine(Trigger());
    }

    private IEnumerator Trigger() {
        yield return new WaitForSeconds(_delayTime);

        var sequence = DOTween.Sequence();
        sequence.Append(_text.DOFade(1, _fadeTime));
    }
}