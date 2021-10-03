using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour {

    private enum NudgeResult {
        NOTHING = 0,
        LOSE = 1,
    }

    [SerializeField] private float _breakThreshold = 4f;
    [SerializeField] private NudgeResult _nudgeResult;
    [SerializeField] private GameObject _debrisObject;

    private GameManager _gameManager;

    private void Awake() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (!col.gameObject.CompareTag("Player")) {
            return;
        }

        if (col.relativeVelocity.magnitude > _breakThreshold) {
            if (_debrisObject != null) {
                var debris = Instantiate(_debrisObject);
                debris.transform.position = transform.position;
            }
            Destroy(col.otherCollider.gameObject);
        }
        else if (_nudgeResult == NudgeResult.LOSE) {
            _gameManager.OnHorseSpotted(gameObject);
        }
    }
}