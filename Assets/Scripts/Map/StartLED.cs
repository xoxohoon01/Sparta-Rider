using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLED : MonoBehaviour
{
    [SerializeField] Material StartLEDBlack;
    [SerializeField] Material StartLEDRed;
    [SerializeField] Material StartLEDGreen;

    [SerializeField] MeshRenderer lightA;
    [SerializeField] MeshRenderer lightB;
    [SerializeField] MeshRenderer lightC;

    WaitForSeconds wait = new WaitForSeconds(1);
    
    void Start()
    {
        StartCoroutine(CoLights());
    }

    public IEnumerator CoLights()
    {
        lightA.material = StartLEDBlack;
        lightB.material = StartLEDBlack;
        lightC.material = StartLEDBlack;
        lightC.material = StartLEDRed;
        yield return wait;
        lightB.material = StartLEDRed;
        yield return wait;
        lightA.material = StartLEDRed;
        yield return wait;
        lightA.material = StartLEDGreen; 
        lightB.material = StartLEDGreen;
        lightC.material = StartLEDGreen;
    }
}
