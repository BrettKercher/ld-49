using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnLevelManager : MonoBehaviour {
    [SerializeField] private DestroyOnHit _winGate;
    [SerializeField] private Winner _win;
    
    // Start is called before the first frame update
    void Awake() {
        _winGate.SubscribeToDestroyEvent(OnWinGateDestroyed);
    }

    private void OnWinGateDestroyed(GameObject gate) {
        StartCoroutine(_win.Trigger());
    }
}