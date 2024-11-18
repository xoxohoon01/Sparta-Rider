using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class CarAI : MonoBehaviour
{
    [FormerlySerializedAs("waypoints")] public Transform[] wayPoints;
    private int wayPointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = wayPoints[wayPointIndex].position;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 5f)
        {
            wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;
            agent.destination = wayPoints[wayPointIndex].position;
        }
    }
}
