using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class destroyOnHit : MonoBehaviour{

    [SerializeField] private float _breakThreshold = 4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D (Collision2D col)
    {
        print(col.relativeVelocity);
        if (col.relativeVelocity.magnitude > _breakThreshold)
        {
            Destroy(col.otherCollider.gameObject);
        }
    }
}
