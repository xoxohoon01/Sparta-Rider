using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    private Rigidbody rb;  // 리지드바디

    public WheelCollider frontLeftWheelCollider;  // 바퀴 콜라이더
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelMesh;  // 바퀴 Mesh
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    private bool isDrifting;
    private float accelInput;
    private float steerInput;

    public float steerForce = 50f;     // 최대 조향 각도
    private float steerMultiplies = 1f;   // 스티어링 계수

    public float acceleration = 100f;    // 현재 회전력 (currentTorque로 변경할 것)
    public float maxSpeed = 20f;                // 최대속도

    public float driftingAxisSpeed = 1;     // 드리프트 감도
    private float driftingAxis;

    [SerializeField] private float driftFriction = 0.5f;   // 드리프트시 마찰
    [SerializeField] private float normalFriction = 1.0f;  // 드리프트 전 마찰

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float throttleInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");
        bool isBoosting = Input.GetKey(KeyCode.LeftShift);
        bool isDrifting = Input.GetKey(KeyCode.Space);

        UpdateSpeed(throttleInput, isBoosting);
        UpdateSteering(steerInput);
        SetTireFriction(isDrifting ? driftFriction : normalFriction);
        ApplyDriftPhysics(steerInput);
    }


    void Update()
    {
        //Vector3 pos;
        //Quaternion quat;
        //frontLeftWheelCollider.GetWorldPose(out pos, out quat);
        //frontRightWheelCollider.GetWorldPose(out pos, out quat);

        //// 바퀴 Mesh 회전 및 위치 업데이트
        //frontLeftWheelMesh.rotation = quat;
        //frontRightWheelMesh.rotation = quat;

        //UpdateAccel();
        //HandBrake();

        if (isDrifting)
        {
            SetTireFriction(driftFriction);
        }
        else
        {
            SetTireFriction(normalFriction);
        }
    }

    void OnAccel(InputValue value)
    {
        accelInput = value.Get<float>();
    }

    void UpdateAccel()
    {
        frontLeftWheelCollider.steerAngle = steerForce * steerMultiplies * steerInput;
        frontRightWheelCollider.steerAngle = steerForce * steerMultiplies * steerInput;

        //frontLeftWheelCollider.motorTorque = acceleration * vertical;
        //frontRightWheelCollider.motorTorque = acceleration * vertical;
        rearLeftWheelCollider.motorTorque = acceleration * accelInput;
        rearRightWheelCollider.motorTorque = acceleration * accelInput;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnSteering(InputValue value)
    {
        steerInput = value.Get<float>();
    }

    void OnBrake(InputValue value)
    {
        isDrifting = value.isPressed;
    }

    void HandBrake()
    {
        if (isDrifting)
        {
            driftingAxis += driftingAxis * Time.deltaTime;

            WheelFrictionCurve frontFriction = frontLeftWheelCollider.forwardFriction;
            frontFriction.extremumSlip = 40f;
            WheelFrictionCurve sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            sidewaysFriction.extremumSlip = 10f;

            // 적용
            frontLeftWheelCollider.forwardFriction = frontFriction;
            frontRightWheelCollider.forwardFriction = frontFriction;
            rearLeftWheelCollider.forwardFriction = frontFriction;
            rearRightWheelCollider.forwardFriction = frontFriction;
            frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
            frontRightWheelCollider.sidewaysFriction = sidewaysFriction;
            rearLeftWheelCollider.sidewaysFriction = sidewaysFriction;
            rearRightWheelCollider.sidewaysFriction = sidewaysFriction;
        }
        else
        {
            WheelFrictionCurve frontFriction = frontLeftWheelCollider.forwardFriction;
            frontFriction.extremumSlip = 0.4f;
            WheelFrictionCurve sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;
            sidewaysFriction.extremumSlip = 0.2f;

            // 적용
            frontLeftWheelCollider.forwardFriction = frontFriction;
            frontRightWheelCollider.forwardFriction = frontFriction;
            rearLeftWheelCollider.forwardFriction = frontFriction;
            rearRightWheelCollider.forwardFriction = frontFriction;
            frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
            frontRightWheelCollider.sidewaysFriction = sidewaysFriction;
            rearLeftWheelCollider.sidewaysFriction = sidewaysFriction;
            rearRightWheelCollider.sidewaysFriction = sidewaysFriction;
        }

        steerMultiplies = isDrifting ? 2 : 1;
    }


    void SetTireFriction(float stiffness)
    {
        WheelFrictionCurve forwardFriction = frontLeftWheelCollider.forwardFriction;
        WheelFrictionCurve sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;

        forwardFriction.stiffness = stiffness;
        sidewaysFriction.stiffness = stiffness;

        frontLeftWheelCollider.forwardFriction = forwardFriction;
        frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        frontRightWheelCollider.forwardFriction = forwardFriction;
        frontRightWheelCollider.sidewaysFriction = sidewaysFriction;
        rearLeftWheelCollider.forwardFriction = forwardFriction;
        rearLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        rearRightWheelCollider.forwardFriction = forwardFriction;
        rearRightWheelCollider.sidewaysFriction = sidewaysFriction;
    }
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float minSteerAngle = 10f;
    [SerializeField] private float speedForMinSteer = 50f;

    void UpdateSteering(float steerInput)
    {
        float speed = rb.velocity.magnitude;
        float steerAngle = Mathf.Lerp(maxSteerAngle, minSteerAngle, speed / speedForMinSteer);

        frontLeftWheelCollider.steerAngle = steerInput * steerAngle;
        frontRightWheelCollider.steerAngle = steerInput * steerAngle;
    }

    [SerializeField] private float boostForce = 5000f;

    void UpdateSpeed(float throttleInput, bool isBoosting)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            float force = isBoosting ? boostForce : throttleInput * acceleration;
            rearLeftWheelCollider.motorTorque = force;
            rearRightWheelCollider.motorTorque = force;
        }
    }

    void ApplyDriftPhysics(float steerInput)
    {
        if (isDrifting)
        {
            Vector3 driftForce = transform.right * steerInput * 2;
            rb.AddForce(driftForce, ForceMode.Acceleration);
        }
    }


}
