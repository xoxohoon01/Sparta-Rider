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
    private float maxSteeringAngle = 25f;
    private float steeringSpeed = 50f;
    private float brakeForce = 300f;

    private bool isAccel;

    public void OnAccel(InputValue value)
    {
        Debug.Log("찍");
        FrontLeftWheel.motorTorque = acceleration * value.Get<float>();
        FrontLeftWheel.brakeTorque = 0;
        FrontRightWheel.motorTorque = acceleration * value.Get<float>();
        FrontRightWheel.brakeTorque = 0;
        BackLeftWheel.motorTorque = acceleration * value.Get<float>();
        BackLeftWheel.brakeTorque = 0;
        BackRightWheel.motorTorque = acceleration * value.Get<float>();
        BackRightWheel.brakeTorque = 0;
    }

    public void OnSteering(InputValue value)
    {
        float steeringAxis = value.Get<float>();
        var steeringAngle = steeringAxis * maxSteeringAngle;
        FrontLeftWheel.steerAngle = Mathf.Lerp(FrontLeftWheel.steerAngle, steeringAngle, steeringSpeed);
        FrontRightWheel.steerAngle = Mathf.Lerp(FrontRightWheel.steerAngle, steeringAngle, steeringSpeed);
    }

    public void OnBrake(InputValue value)
    {
        FrontLeftWheel.brakeTorque = brakeForce;
        FrontRightWheel.brakeTorque = brakeForce;
        BackLeftWheel.brakeTorque = brakeForce;
        BackRightWheel.brakeTorque = brakeForce;
    }


}
