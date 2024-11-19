using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVehicleSystem : MonoBehaviour
{
    Rigidbody carRigidbody;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float driftPower = 10f;
    [SerializeField] private float driftControl = 0.9f;

    private bool isDrifting;
    [SerializeField] private float boostForce = 50f;
    [SerializeField] private float boostDuration = 2f;
    private bool isBoosting;

    IEnumerator Boost()
    {
        isBoosting = true;
        float originalAcceleration = acceleration;
        acceleration += boostForce;

        yield return new WaitForSeconds(boostDuration);

        acceleration = originalAcceleration;
        isBoosting = false;
    }

    void CheckBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting)
        {
            StartCoroutine(Boost());
        }
    }

    void UpdateDrift(bool driftInput)
    {
        if (driftInput)
        {
            isDrifting = true;
            Vector3 driftForce = transform.right * Input.GetAxis("Horizontal") * driftPower;
            carRigidbody.AddForce(driftForce, ForceMode.Acceleration);

            // 이동 방향과 차량 회전을 자연스럽게 분리
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, transform.forward * carRigidbody.velocity.magnitude, driftControl * Time.deltaTime);
        }
        else
        {
            isDrifting = false;
        }
    }

    void UpdateSteering()
    {
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, steerInput * turnSpeed * Time.deltaTime);
    }

    void UpdateMovement()
    {
        float throttleInput = Input.GetAxis("Vertical");
        Vector3 forwardForce = transform.forward * throttleInput * acceleration;

        if (carRigidbody.velocity.magnitude < maxSpeed)
            carRigidbody.AddForce(forwardForce, ForceMode.Acceleration);
    }

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.mass = 800f; // 가벼운 카트 느낌
        carRigidbody.drag = 0.1f; // 공기 저항
        carRigidbody.angularDrag = 2f; // 회전 저항
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0); // 무게 중심
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateSteering();

        bool driftInput = Input.GetKey(KeyCode.Space);
        UpdateDrift(driftInput);

        CheckBoost();
    }

}
