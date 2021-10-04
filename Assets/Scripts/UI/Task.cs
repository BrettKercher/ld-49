using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Task : MonoBehaviour {
    void Start() {
        transform.DOScaleY(0, 0.25f).From();
    }
}