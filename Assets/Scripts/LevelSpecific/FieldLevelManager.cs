using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldLevelManager : MonoBehaviour {

    [SerializeField] private DestroyOnHit _guardSmoosh;
    [SerializeField] private DestroyOnHit _winGate;

    [SerializeField] private Winner _win;


    // Start is called before the first frame update
    void Awake() {
        _guardSmoosh.SubscribeToDestroyEvent(OnGuardSmooshed);
        _winGate.SubscribeToDestroyEvent(OnWinGateDestroyed);
    }

    private void OnGuardSmooshed(GameObject guard) {
        Debug.Log("GuardSmooshed!");
    }
    
    private void OnWinGateDestroyed(GameObject gate) {
        StartCoroutine(_win.Trigger());
    }
    
    public void RetryOnClick(){
        Application.LoadLevel(Application.loadedLevel);
    }
    public void NextLevelOnClick()
    {
        SceneManager.LoadScene(0);//ints and such also work.
    }
}