using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{    
    // Start is called before the first frame update
    void Awake()
    {
        GameObject car = Resources.Load<GameObject>($"Cars/Car_NewVehicle_{GameManager.Instance.carNumber.ToString()}");
        Instantiate(car, transform.position, transform.rotation);
    }
}
