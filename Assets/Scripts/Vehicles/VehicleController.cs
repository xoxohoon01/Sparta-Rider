using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class VehicleController : MonoBehaviour
{
    public VehicleStatus status;
    public float accelerationMultiplier;        // 가속도 계수
    public float maxSpeed;                      // 최대속도
    public float maxSteeringAngle;              // 조향 계수
    public float steeringSpeed;                 // 조향 속도
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

    public float carSpeed;          //현재 속도 (UI에 표시될 속도)

    private Rigidbody carRigidbody;
    public float throttleAxis;     // 현재 가속 값
    public float steeringAxis;     // 현재 스티어링 값
    public float driftingAxis;     // 현재 드리프트 계수값 (0.0 ~ 1.0)
    private float localVelocityZ;   // 차 기준 Z축으로 가해지는 힘 (드리프트)
    private float localVelocityX;   // 차 기준 X축으로 가해지는 힘 (드리프트)
    private bool decelerationCar;   // 차가 감속중인지 판별 (true: 차 속도가 줄어듬)

    // 휠 콜라이더별 마찰력 컴포넌트
    WheelFrictionCurve FLwheelFriction;
    float FLWextremumSlip;
    WheelFrictionCurve FRwheelFriction;
    float FRWextremumSlip;
    WheelFrictionCurve RLwheelFriction;
    float RLWextremumSlip;
    WheelFrictionCurve RRwheelFriction;
    float RRWextremumSlip;

    float throttleInputAxis;        // W, S 누를 시 리턴 값 (-1, 0, 1)
    float steeringInputAxis;        // A, D 누를 시 리턴 값 (-1, 0, 1)
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

        // 차체 성능 초기화
        carRigidbody.mass = status.mass;
        maxSpeed = status.maxSpeed;
        maxSteeringAngle = status.maxSteeringAngle;
        steeringSpeed = status.steeringSpeed;
        accelerationMultiplier = status.accelerationMultiplier;
        decelerationMultiplier = status.decelerationMultiplier;
        handbrakeDriftMultiplier = status.handbrakeDriftMultiplier;

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
        if (isBrake)
        {
            CancelInvoke("DecelerateCar");
            Handbrake();
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

        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;

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

    // 드리프트 끝나고 휠 콜라이더 마찰력 계수 복귀
    public void RecoverTraction()
    {
        // 드리프트 계수 점차 감소 (최소인 0까지)
        driftingAxis = driftingAxis - (Time.deltaTime / 1.5f);
        if (driftingAxis < 0f)
        {
            driftingAxis = 0f;
        }

        // 현재 마찰력 계수가 
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

        if (!value.isPressed)
        {
            RecoverTraction();
        }
    }
    
    // 드리프트
    public void Handbrake()
    {
        CancelInvoke("RecoverTraction");

        // 드리프트 유지 중, 드리프트 계수 점차 상승
        driftingAxis = driftingAxis + (Time.deltaTime);
        float secureStartingPoint = driftingAxis * FLWextremumSlip * handbrakeDriftMultiplier;

        // 드리프트 시작 시 이동 방향과 힘의 방향 고려하여 드리프트 계수 선택 (0 < 계수 < 1)
        if (secureStartingPoint < FLWextremumSlip)
        {
            driftingAxis = FLWextremumSlip / (FLWextremumSlip * handbrakeDriftMultiplier);
        }

        // 드리프트 계수가 1보다 많을 경우 1로 고정
        if (driftingAxis > 1f)
        {
            driftingAxis = 1f;
        }

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

    }
}
