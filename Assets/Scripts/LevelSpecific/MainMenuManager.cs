using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public void PlayOnClick(){
        SceneManager.LoadScene(1);
    }

}