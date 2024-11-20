using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle", menuName = "NewVehicle")]
public class VehicleStatus : ScriptableObject
{
    public float mass = 900f;                        // 차체 무게
    public float accelerationMultiplier = 6f;        // 가속력
    public float maxSpeed = 120f;                    // 최대 속도
    public float maxSteeringAngle = 45f;             // 조향 민감도
    public float steeringSpeed = 0.5f;               // 조향 민감도
    public float decelerationMultiplier = 0.1f;      // 감속 속도
    public float handbrakeDriftMultiplier = 6f;      // 드리프트 계수
}
