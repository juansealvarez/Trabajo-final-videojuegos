using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EESongInteractable : MonoBehaviour
{
    [System.NonSerialized]
    public BoxCollider boxCollider;
    public bool Interacted = false;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
}
