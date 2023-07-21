using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public GameObject buttonReturn;
    void Start()
    {
        StartCoroutine(enableButton());
    }

    IEnumerator enableButton()
    {
        yield return new WaitForSeconds(5f);
        buttonReturn.SetActive(true);
    }
}
