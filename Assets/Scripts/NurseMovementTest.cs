using UnityEngine;

using UnityEngine;
using UnityEngine.AI;

public class NurseMovementTest : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogWarning("Check nurse NavMesh and target!");
        }
    }
}