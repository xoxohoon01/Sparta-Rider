using System.Collections;
using System.Collections.Generic;
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

    public float motorTorque = 200f;      // 엔진 힘
    public float steerForce = 50f;     // 최대 조향 각도
    private float steerMultiplies = 1f;   // 스티어링 계수

    public float acceleration = 200f;            // 가속도
    public float maxSpeed = 500f;                // 최대속도


    [SerializeField] private float driftFriction = 0.5f;   // 드리프트시 마찰
    [SerializeField] private float normalFriction = 1.0f;  // 드리프트 전 마찰

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 입력 받기
        float vertical = Input.GetAxis("Vertical"); // 전진/후진
        float horizontal = Input.GetAxis("Horizontal"); // 좌/우 회전

        frontLeftWheelCollider.steerAngle = steerForce * steerMultiplies * horizontal;
        frontRightWheelCollider.steerAngle = steerForce * steerMultiplies * horizontal;
        //frontLeftWheelCollider.steerAngle = maxSteerAngle * steerMultiplies * horizontal;
        //frontRightWheelCollider.steerAngle = maxSteerAngle * steerMultiplies * horizontal;

        frontLeftWheelCollider.motorTorque = acceleration * vertical;
        frontRightWheelCollider.motorTorque = acceleration * vertical;
        rearLeftWheelCollider.motorTorque = acceleration * vertical;
        rearRightWheelCollider.motorTorque = acceleration * vertical;
    }

    void Update()
    {
        Vector3 pos;
        Quaternion quat;
        frontLeftWheelCollider.GetWorldPose(out pos, out quat);
        frontRightWheelCollider.GetWorldPose(out pos, out quat);

        // 바퀴 Mesh 회전 및 위치 업데이트
        frontLeftWheelMesh.rotation = quat;
        frontRightWheelMesh.rotation = quat;
    }

    void OnBrake(InputValue value)
    {
        bool isDrifting = value.isPressed;

        WheelFrictionCurve forwardFriction = frontLeftWheelCollider.forwardFriction;
        WheelFrictionCurve sidewaysFriction = frontLeftWheelCollider.sidewaysFriction;

        float targetFriction = isDrifting ? driftFriction : normalFriction;
        rearLeftWheelCollider.brakeTorque = isDrifting ? 100f : 0;
        rearRightWheelCollider.brakeTorque = isDrifting ? 100f : 0;
        steerMultiplies = isDrifting ? 1.5f : 1.0f;

        forwardFriction.stiffness = targetFriction;
        sidewaysFriction.stiffness = targetFriction;

        // 적용
        frontLeftWheelCollider.forwardFriction = forwardFriction;
        frontLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        frontRightWheelCollider.forwardFriction = forwardFriction;
        frontRightWheelCollider.sidewaysFriction = sidewaysFriction;
        rearLeftWheelCollider.forwardFriction = forwardFriction;
        rearLeftWheelCollider.sidewaysFriction = sidewaysFriction;
        rearRightWheelCollider.forwardFriction = forwardFriction;
        rearRightWheelCollider.sidewaysFriction = sidewaysFriction;

        
    }

}
