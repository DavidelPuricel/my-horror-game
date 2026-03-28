using UnityEngine;
using UnityEngine.AI;

public class ChaseController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;

    private bool isChasing = false;

    private void Start()
    {
        if (agent != null)
            agent.isStopped = true;
    }

    private void Update()
    {
        if (!isChasing || player == null || agent == null)
            return;

        agent.SetDestination(player.position);
    }

    public void BeginChase()
    {
        isChasing = true;

        if (agent != null)
            agent.isStopped = false;
    }

    public void StopChase()
    {
        isChasing = false;

        if (agent != null)
            agent.isStopped = true;
    }
}
