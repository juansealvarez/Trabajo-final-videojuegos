using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaSpawn : MonoBehaviour
{
    private Rigidbody mRb;
    private CapsuleCollider mCollider;
    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        mCollider = GetComponent<CapsuleCollider>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Suelo"))
        {
            Destroy(mRb);
            transform.position = new Vector3(transform.position.x, transform.position.y+0.15f, transform.position.z);
            //mCollider.isTrigger = true;
        }
    }
}
