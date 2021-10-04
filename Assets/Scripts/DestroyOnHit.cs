using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour {

    private enum NudgeResult {
        NOTHING = 0,
        LOSE = 1,
    }

    [SerializeField] private float _breakThreshold = 3f;
    [SerializeField] private NudgeResult _nudgeResult;
    [SerializeField] private GameObject _debrisObject;
    [SerializeField] private GameObject _debrisObject2;
	[SerializeField] private ParticleSystem _particleSystem;

    private GameManager _gameManager;
    
    public delegate void OnDestroyHandler(GameObject destructee);
    public event OnDestroyHandler Destroyed;

    private void Awake() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_particleSystem = GetComponent<ParticleSystem>();
    }
    
    public void SubscribeToDestroyEvent(OnDestroyHandler onDestroyFunction) {
        Destroyed += onDestroyFunction;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (!col.gameObject.CompareTag("Player")) {
            return;
        }

        if (col.relativeVelocity.magnitude > _breakThreshold) {
            if (_debrisObject != null) {
                var debris = Instantiate(_debrisObject);
                debris.transform.position = transform.position;
                debris.transform.rotation = transform.rotation;
            }
			if (_debrisObject2 != null) {
                var debris2 = Instantiate(_debrisObject2);
                debris2.transform.position = transform.position;
                debris2.transform.rotation = transform.rotation;
            }
            Destroyed?.Invoke(gameObject);
            Destroy(col.otherCollider.gameObject);
        }
        else if (_nudgeResult == NudgeResult.LOSE) {
            _gameManager.OnHorseSpotted(gameObject);
        }
    }
}