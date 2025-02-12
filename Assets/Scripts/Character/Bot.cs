using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Bot : Character
{
    public Transform[] waypoints; // Danh sách điểm đến
    private int currentWaypointIndex = 0; // Chỉ mục điểm đến hiện tại
    private NavMeshAgent agent; // NavMeshAgent

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForWaypoints());
    }

    IEnumerator WaitForWaypoints()
    {
        // Chờ đến khi MapManager tạo xong Waypoints
        while (MapManager.instance == null || MapManager.instance.waypoints.Count == 0)
        {
            yield return null; // Đợi 1 frame
        }

        // Gán danh sách Waypoints từ MapManager
        waypoints = MapManager.instance.waypoints.ToArray();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Lặp lại
    }
}
