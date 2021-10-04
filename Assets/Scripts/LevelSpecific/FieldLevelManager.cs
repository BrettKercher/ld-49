using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldLevelManager : MonoBehaviour {

    [SerializeField] private DestroyOnHit _winGate;

    [SerializeField] private Winner _win;

    // Start is called before the first frame update
    private void Awake() {
        _winGate.SubscribeToDestroyEvent(OnWinGateDestroyed);
    }

    private void OnWinGateDestroyed(GameObject gate) {
        StartCoroutine(_win.Trigger());
    }

    public void RetryOnClick()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        //Debug.Log("im here");
    }

    public void NextLevelOnClick() {
        SceneManager.LoadScene(2);//ints and such also work.
    }
}