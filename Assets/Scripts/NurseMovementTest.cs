
using UnityEngine;
using UnityEngine.AI;

public class NurseMovementTest : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool playerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Nurse is not on the NavMesh!");
            return;
        }

        agent.speed = patrolSpeed;

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
            return;

        // Player detection
        bool inRange = player != null &&
            Vector3.Distance(transform.position, player.position)
            <= detectionRange;

        if (inRange != playerDetected)
        {
            playerDetected = inRange;

            if (playerDetected)
            {
                Debug.Log("Nurse detected the player!");
            }
            else
            {
                Debug.Log("Nurse lost the player!");

                agent.speed = patrolSpeed;

                if (waypoints.Length > 0)
                {
                    agent.SetDestination(
                        waypoints[currentWaypointIndex].position
                    );
                }
            }
        }

        // Chase the player
        if (playerDetected)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            return;
        }

        // Continue normal patrol
        if (waypoints.Length == 0)
            return;

        if (!agent.pathPending && agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            currentWaypointIndex =
                (currentWaypointIndex + 1) % waypoints.Length;

            agent.SetDestination(
                waypoints[currentWaypointIndex].position
            );
        }
    }
}
