using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour {

    [SerializeField] private GameObject _taskPrefab;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            AddTask();
        }
    }

    public void AddTask() {
        var taskGO = Instantiate(_taskPrefab, transform);
    }
}