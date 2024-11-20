using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class NewVehicleController : MonoBehaviour
{
    public float maxSpeed;      // 최대속도
    public float accelerationMultiplier;        // 가속도 계수
    public float maxSteeringAngle;      // 조향 계수
    public float steeringSpeed;     // 조향 속도

    public float brakeForce;        // 브레이크 힘
    public float decelerationMultiplier;        // 감속 계수
    public float handbrakeDriftMultiplier;      // 드리프트 계수

    public GameObject frontLeftMesh;
    public GameObject frontRightMesh;
    public GameObject rearLeftMesh;
    public GameObject rearRightMesh;
    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    public float carSpeed;
    public bool isDrifting;
    public bool isTractionLocked;

    private Rigidbody carRigidbody;
    private float throttleAxis;     // W, S 누를 시 리턴값 (-1, 0, 1)
    private float steeringAxis;     // A, D 누를 시 리턴값 (-1, 0, 1)
    private float driftingAxis;     // 드리프트시 리턴값 (0, 1)
    private float localVelocityZ;   // 차 기준 Z축으로 가해지는 힘
    private float localVelocityX;   // 차 기준 X축으로 가해지는 힘
    private bool decelerationCar;   // 차가 감속중인지 판별

    // 휠 콜라이더별 마찰력 컴포넌트
    WheelFrictionCurve FLwheelFriction;
    float FLWextremumSlip;
    WheelFrictionCurve FRwheelFriction;
    float FRWextremumSlip;
    WheelFrictionCurve RLwheelFriction;
    float RLWextremumSlip;
    WheelFrictionCurve RRwheelFriction;
    float RRWextremumSlip;

    float throttleInputAxis;
    float steeringInputAxis;
    bool isBrake;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>(); // 리지드바디 초기화

        #region 휠 콜라이더 마찰력 관련
        // 전방 좌측
        FLwheelFriction = new WheelFrictionCurve();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;

        // 전방 우측
        FRwheelFriction = new WheelFrictionCurve();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;

        // 후방 좌측
        RLwheelFriction = new WheelFrictionCurve();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;

        // 후방 우측
        RRwheelFriction = new WheelFrictionCurve();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;
        #endregion

    }

    private void Update()
    {
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
        localVelocityX = transform.InverseTransformDirection(carRigidbody.velocity).x;
        localVelocityZ = transform.InverseTransformDirection(carRigidbody.velocity).z;

        // 전진, 후진 관련
        if (throttleInputAxis != 0)
        {
            CancelInvoke("DecelerateCar");
            decelerationCar = false;
            AccelerateCar();
        }
        if (throttleInputAxis == 0)
        {
            ThrottleOff();
        }

        // 스티어링 관련
        if (steeringInputAxis != 0)
        {
            SteeringCar();
        }
        if (steeringInputAxis == 0)
        {
            ResetSteeringCar();
        }

        // 드리프트 관련
        if (UnityEngine.Input.GetKey(KeyCode.Space))
        {
            CancelInvoke("DecelerateCar");
            Handbrake();
        }
        if (UnityEngine.Input.GetKeyUp(KeyCode.Space))
        {
            RecoverTraction();
        }

        // 감속 관련
        if (throttleInputAxis == 0 && steeringInputAxis == 0 && isBrake == false && decelerationCar == false)
        {
            InvokeRepeating("DecelerateCar", 0, 0.1f);
            decelerationCar = true;
        }

        AnimateWheelMeshes();
    }

    public void OnAccel(InputValue value)
    {
        throttleInputAxis = value.Get<float>();
    }
    private void AccelerateCar()
    {
        if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxSpeed)
        {
            throttleAxis = Mathf.Min(throttleAxis + Time.deltaTime, 1);
            frontLeftCollider.brakeTorque = 0;
            frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis * throttleInputAxis;
            frontRightCollider.brakeTorque = 0;
            frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis * throttleInputAxis;
            rearLeftCollider.brakeTorque = 0;
            rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis * throttleInputAxis;
            rearRightCollider.brakeTorque = 0;
            rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis * throttleInputAxis;
        }
        else
        {
            frontLeftCollider.brakeTorque = 0;
            frontRightCollider.brakeTorque = 0;
            rearLeftCollider.brakeTorque = 0;
            rearRightCollider.brakeTorque = 0;
        }
    }
    private void ThrottleOff()
    {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
    }

    // 감속
    public void DecelerateCar()
    {
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        if (throttleAxis != 0f)
        {
            if (throttleAxis > 0f)
            {
                throttleAxis = throttleAxis - (Time.deltaTime * 10f);
            }
            else if (throttleAxis < 0f)
            {
                throttleAxis = throttleAxis + (Time.deltaTime * 10f);
            }
            if (Mathf.Abs(throttleAxis) < 0.15f)
            {
                throttleAxis = 0f;
            }
        }
        carRigidbody.velocity = carRigidbody.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
        // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
        // If the magnitude of the car's velocity is less than 0.25f (very slow velocity), then stop the car completely and
        // also cancel the invoke of this method.
        if (carRigidbody.velocity.magnitude < 0.25f)
        {
            carRigidbody.velocity = Vector3.zero;
            CancelInvoke("DecelerateCar");
        }
    }

    public void OnSteering(InputValue value)
    {
        steeringInputAxis = value.Get<float>();
    }

    private void SteeringCar()
    {
        steeringAxis = Mathf.Clamp(steeringAxis + (Time.deltaTime * steeringInputAxis), -1f, 1f);
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    private void ResetSteeringCar()
    {
        if (steeringAxis < 0f)
        {
            steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        }
        else if (steeringAxis > 0f)
        {
            steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        }
        if (Mathf.Abs(frontLeftCollider.steerAngle) < 1f)
        {
            steeringAxis = 0f;
        }
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    // 휠 메쉬 회전시키기
    void AnimateWheelMeshes()
    {
        try
        {
            Quaternion FLWRotation;
            Vector3 FLWPosition;
            frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
            frontLeftMesh.transform.position = FLWPosition;
            frontLeftMesh.transform.rotation = FLWRotation;

            Quaternion FRWRotation;
            Vector3 FRWPosition;
            frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
            frontRightMesh.transform.position = FRWPosition;
            frontRightMesh.transform.rotation = FRWRotation;

            Quaternion RLWRotation;
            Vector3 RLWPosition;
            rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
            rearLeftMesh.transform.position = RLWPosition;
            rearLeftMesh.transform.rotation = RLWRotation;

            Quaternion RRWRotation;
            Vector3 RRWPosition;
            rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
            rearRightMesh.transform.position = RRWPosition;
            rearRightMesh.transform.rotation = RRWRotation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    // 수정할 것 1
    public void RecoverTraction()
    {
        isTractionLocked = false;
        driftingAxis = driftingAxis - (Time.deltaTime / 1.5f);
        if (driftingAxis < 0f)
        {
            driftingAxis = 0f;
        }

        //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
        //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
        // car's grip.
        if (FLwheelFriction.extremumSlip > FLWextremumSlip)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            rearRightCollider.sidewaysFriction = RRwheelFriction;

            Invoke("RecoverTraction", Time.deltaTime);

        }
        else if (FLwheelFriction.extremumSlip < FLWextremumSlip)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip;
            rearRightCollider.sidewaysFriction = RRwheelFriction;

            driftingAxis = 0f;
        }
    }

    public void OnBrake(InputValue value)
    {
        isBrake = value.isPressed;
    }
    // 수정할 것 2
    public void Handbrake()
    {
        CancelInvoke("RecoverTraction");
        // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
        // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
        // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
        driftingAxis = driftingAxis + (Time.deltaTime);
        float secureStartingPoint = driftingAxis * FLWextremumSlip * handbrakeDriftMultiplier;

        if (secureStartingPoint < FLWextremumSlip)
        {
            driftingAxis = FLWextremumSlip / (FLWextremumSlip * handbrakeDriftMultiplier);
        }
        if (driftingAxis > 1f)
        {
            driftingAxis = 1f;
        }
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car lost its traction, then the car will start emitting particle systems.
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }
        //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
        //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
        // = 1f.
        if (driftingAxis < 1f)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
            rearRightCollider.sidewaysFriction = RRwheelFriction;
        }

        // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
        // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
        isTractionLocked = true;

    }
}
