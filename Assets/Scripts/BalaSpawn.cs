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
    /*private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Ground"))
        {
            mRb.isKinematic = true;
            mCollider.isTrigger = true;
        }
    }*/
}
