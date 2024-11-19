using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle", menuName = "NewVehicle")]
public class VehicleStatus : ScriptableObject
{
    public float currentSpeed;      // 현재 속도
    public float acceleration = 50f; // 가속력
    public float maxSpeed = 20f; // 최대 속도
    public float steering = 100f; // 조향 민감도
    public float driftForce = 15f; // 드리프트 힘
    public float driftTurnMultiplier = 1.5f; // 드리프트 중 조향 민감도 증가
    public float deceleration = 10f; // 자연 감속력
}
