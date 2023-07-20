using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Round : MonoBehaviour
{
    private Animator mAnimator;
    
    void Start()
    {
        Debug.Log(StateNameController.isStartRound);
        mAnimator = GetComponent<Animator>();
        if(!StateNameController.isStartRound){
            StateNameController.isStartRound = true;
            mAnimator.SetTrigger("IsStart");
        }
    }



}
