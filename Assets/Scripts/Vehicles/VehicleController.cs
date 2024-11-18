using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float acceleration = 50f; // 가속력
    public float maxSpeed = 20f; // 최대 속도
    public float steering = 100f; // 조향 민감도
    public float driftForce = 15f; // 드리프트 힘
    public float driftTurnMultiplier = 1.5f; // 드리프트 중 조향 민감도 증가
    public float deceleration = 10f; // 자연 감속력

    private Rigidbody rb;
    private bool isDrifting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleDrift();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // 사용자 입력
        float moveInput = Input.GetAxis("Vertical"); // 전진/후진 (W/S 또는 ↑/↓)
        float turnInput = Input.GetAxis("Horizontal"); // 좌우 조향 (A/D 또는 ←/→)

        // 가속 및 감속
        if (moveInput != 0)
        {
            rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Acceleration);
        }
        else
        {
            // 자연 감속
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // 속도 제한
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // 현재 속도 계산 (전진: 양수, 후진: 음수)
        float currentSpeed = Vector3.Dot(rb.velocity, transform.forward);
        //Debug.Log(currentSpeed / maxSpeed);
        // 조향 (속도가 일정 이상일 때만 작동)
        if (Mathf.Abs(currentSpeed) > 0.1f) // 절대값 속도로 움직임 확인
        {
            float steerAmount = turnInput * steering * (currentSpeed / maxSpeed) * Time.fixedDeltaTime;

            if (isDrifting)
            {
                steerAmount *= driftTurnMultiplier; // 드리프트 중엔 더 민감하게 조향
            }

            transform.Rotate(Vector3.up, steerAmount);
        }

        // 시각적 회전 (속도에 따라 바퀴 회전)
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Wheel")) // 바퀴 이름에 따라 시각적으로 회전
            {
                child.Rotate(Vector3.right * Mathf.Abs(currentSpeed) * Time.fixedDeltaTime * 10f);
            }
        }
    }


    void HandleDrift()
    {
        // 드리프트 시작
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.magnitude > 5f)
        {
            isDrifting = true;
        }

        // 드리프트 중 물리 효과 적용
        if (isDrifting)
        {
            Vector3 driftDirection = transform.right * Input.GetAxis("Horizontal") * driftForce;
            //rb.AddForce(driftDirection, ForceMode.Acceleration);

            // 드리프트 종료
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isDrifting = false;
            }
        }
    }
}
