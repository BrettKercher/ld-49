using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PatrolAction/BasicRotate")]
public class BasicRotatePatrolAction : PatrolArriveAction {

    [Serializable]
    public struct Rotation {
        public float time;
        public float angle;
        public float delay;
    }

    [SerializeField] private Rotation[] rotations;

    public override IEnumerator Execute(GameObject target) {

        var rotateSequence = DOTween.Sequence();

        var totalTime = 0f;
        foreach (var rotation in rotations) {
            rotateSequence.Insert(totalTime, target.transform.DORotate(new Vector3(0, 0, rotation.angle), rotation.time));
            totalTime += rotation.delay + rotation.time;
        }

        yield return rotateSequence.WaitForCompletion();
        rotateSequence.Kill();

        yield return new WaitForSeconds(rotations.Last().delay);

        OnActionComplete();
    }
}
