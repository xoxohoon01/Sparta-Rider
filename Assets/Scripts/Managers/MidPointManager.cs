using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MidPointManager : DestroySingleton<MidPointManager>
{
    private Transform defaultSpawnPoint; // 초기 스폰 포인트
    private Transform lastPassedMidPoint; // 마지막으로 통과한 중간 포인트
    private HashSet<int> passedMidPoints = new HashSet<int>(); // 통과한 중간 포인트
    private int currentLap = 0; // 현재 랩
    private int totalLaps = 3; // 총 랩 수
    private List<MidPoint> midPoints = new List<MidPoint>(); // 모든 중간 포인트

    protected override void Awake()
    {
        base.Awake();

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


    public void PassMidpoint(int id, Transform midPointTransform)
    {
        if (!passedMidPoints.Contains(id))
        {
            passedMidPoints.Add(id);
            lastPassedMidPoint = midPointTransform; // 마지막 통과한 중간 포인트 갱신
            Debug.Log(id);
        }
    }

    public void CheckFinishLine()
    {
        if (passedMidPoints.Count == midPoints.Count)
        {
            currentLap++;
            passedMidPoints.Clear(); // 중간 포인트 초기화

            if (currentLap >= totalLaps)
            {
                OnRaceFinished();
            }
        }
    }

    private void OnRaceFinished()
    {
        // 레이스가 끝났을 때의 처리 로직
        Debug.Log("Race Ended!");
        // TODO : 게임 결과 UI 표시, 플레이어 제어 비활성화
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
}
