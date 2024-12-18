using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLED : MonoBehaviour
{
    [SerializeField] Material StartLEDBlack;
    [SerializeField] Material StartLEDRed;
    [SerializeField] Material StartLEDGreen;

    [SerializeField] MeshRenderer lightA;
    [SerializeField] MeshRenderer lightB;
    [SerializeField] MeshRenderer lightC;

    WaitForSeconds wait = new WaitForSeconds(1);
    private VehicleController player;
    private float original;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MapEditorScene")
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleController>();
            original = player.itemAccelerationMultiplier;
            StartCoroutine(CoLights());
        }
    }

    public IEnumerator CoLights()
    {
        player.itemAccelerationMultiplier = 0;
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
        player.itemAccelerationMultiplier = original;
        MidPointManager.Instance.lapStartTime = Time.time;
    }

}
