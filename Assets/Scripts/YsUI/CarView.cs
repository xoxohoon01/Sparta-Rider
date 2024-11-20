using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarView : MonoBehaviour
{
    private float rotationSpeed = 10;
    private float firstIndex = 222;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed, Space.World);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            transform.DORotate(new Vector3(0, firstIndex, 0), 0.5f);
        }
    }
}
