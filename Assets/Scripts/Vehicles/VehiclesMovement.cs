using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehiclesMovement : MonoBehaviour
{
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider BackLeftWheel;
    public WheelCollider BackRightWheel;

    private float acceleration = 100f;
    private float nowAcceleration = 0f;

    private void OnAcecl(InputValue value)
    {
        if (value.isPressed)
        {
            FrontLeftWheel.motorTorque = acceleration;
            FrontLeftWheel.brakeTorque = 0;
            FrontRightWheel.motorTorque = acceleration;
            FrontRightWheel.brakeTorque = 0;
            BackLeftWheel.motorTorque = acceleration;
            BackLeftWheel.brakeTorque = 0;
            BackRightWheel.motorTorque = acceleration;
            BackRightWheel.brakeTorque = 0;
        }
        else
        {
            FrontLeftWheel.motorTorque = 0;
            FrontLeftWheel.brakeTorque = 0;
            FrontRightWheel.motorTorque = 0;
            FrontRightWheel.brakeTorque = 0;
            BackLeftWheel.motorTorque = 0;
            BackLeftWheel.brakeTorque = 0;
            BackRightWheel.motorTorque = 0;
            BackRightWheel.brakeTorque = 0;
        }
    }

    private void OnBrake(InputValue value)
    {

    }

    private void Update()
    {
    }
}
