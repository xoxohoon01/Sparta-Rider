using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{    
    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "MapEditorScene")
        {
            GameObject car = Resources.Load<GameObject>($"Cars/Car_NewVehicle_{GameManager.Instance.carNumber.ToString()}");
            Instantiate(car, transform.position, transform.rotation);
        }
    }
}
