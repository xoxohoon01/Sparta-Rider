using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : MonoBehaviour
{
    private float rotationSpeed = 10;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed, Space.World);
        }
    }
}
