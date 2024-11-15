using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLED : MonoBehaviour
{
    [SerializeField] Material StartLEDRed;
    [SerializeField] Material StartLEDGreen;

    [SerializeField] MeshRenderer lightA;
    [SerializeField] MeshRenderer lightB;
    [SerializeField] MeshRenderer lightC;

    WaitForSeconds wait = new WaitForSeconds(1);
    
    void Start()
    {
        StartCoroutine(ChangeLights());
    }

    IEnumerator ChangeLights()
    {
        lightA.material = StartLEDRed;
        yield return wait;
        lightB.material = StartLEDRed;
        yield return wait;
        lightC.material = StartLEDRed;
        yield return wait;
        lightA.material = StartLEDGreen; 
        lightB.material = StartLEDGreen;
        lightC.material = StartLEDGreen;
    }
}
