using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MidPointManager : DestroySingleton<MidPointManager>
{
    private Transform defaultSpawnPoint; // 초기 스폰 포인트
    private Transform lastPassedMidPoint; // 마지막으로 통과한 중간 포인트
    private HashSet<int> passedMidPoints = new HashSet<int>(); // 통과한 중간 포인트
    public int currentLap = 1; // 현재 랩
    public int totalLaps = 3; // 총 랩 수
    public float lapStartTime; // 랩 시작 시간;
    public float currentLapTime;  // 현재 랩 소요시간
    public float bestLapTime;  // 최고 기록
    public bool isClear;  // 총 랩 도달시

    private List<MidPoint> midPoints = new List<MidPoint>(); // 모든 중간 포인트

    protected override void Awake()
    {
        base.Awake();

        // bestLapTime, isClear 초기화
        bestLapTime = float.MaxValue;
        isClear = false;

        // 기본 스폰 포인트 설정 ("DefaultSpawn" 태그 사용)
        defaultSpawnPoint = GameObject.FindWithTag("DefaultSpawn").transform;
        if (defaultSpawnPoint == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                defaultSpawnPoint = playerObject.transform;
            }
            else
            {
                return; // 초기화 실패 시 이후 로직 중단
            }
        }

        // 모든 중간 포인트 동적으로 검색
        GameObject[] midpointObjects = GameObject.FindGameObjectsWithTag("MidPoint");

        for (int i = 0; i < midpointObjects.Length; i++)
        {
            MidPoint midPoint = midpointObjects[i].GetComponent<MidPoint>();
            if (midPoint != null)
            {
                midPoint.SetId(i); // 고유 ID 설정
                midPoints.Add(midPoint);
            }
        }

        // 마지막 스폰 포인트를 기본 스폰 포인트로 초기화
        lastPassedMidPoint = defaultSpawnPoint;
    }

    private void Update()
    {
        currentLapTime = Time.time - lapStartTime;
    }


    public void PassMidpoint(int id, Transform midPointTransform)
    {
        if (!passedMidPoints.Contains(id))
        {
            passedMidPoints.Add(id);
            lastPassedMidPoint = midPointTransform; // 마지막 통과한 중간 포인트 갱신
        }
    }

    public void CheckFinishLine()
    {
        if (passedMidPoints.Count == midPoints.Count)
        {
            currentLap++;
            lapStartTime = Time.time; // 랩 시작 시간 초기화
            if(currentLapTime < bestLapTime)  // 최고기록 판별
            {
                bestLapTime = currentLapTime;
            }

            passedMidPoints.Clear(); // 중간 포인트 초기화

            if (currentLap > totalLaps)
            {
                currentLap--; // 표시는 totalLaps로
                OnRaceFinished();
            }
        }
    }

    private void OnRaceFinished()
    {
        // 레이스가 끝났을 때의 처리 로직
        Debug.Log("Race Ended!");
        // TODO : 게임 결과 UI 표시, 플레이어 제어 비활성화
        isClear = true;
        Time.timeScale = 0f;
    }

    public void RespawnPlayer(GameObject player)
    {
        Transform respawnPoint = lastPassedMidPoint != null ? lastPassedMidPoint : defaultSpawnPoint;

        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;

        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ResetTimer()
    {
        currentLapTime = 0; // 현재 시간을 기준으로 초기화
    }
}
