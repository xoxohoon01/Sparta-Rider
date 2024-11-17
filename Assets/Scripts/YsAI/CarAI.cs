using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public Transform[] waypoints; // 트랙의 Waypoints
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = waypoints[currentWaypointIndex].position; // 첫 목적지 설정
    }

    void Update()
    {
        // 목적지에 도착했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 5f)
        {
            // 다음 Waypoint로 이동
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.destination = waypoints[currentWaypointIndex].position;
        }
    }
}
