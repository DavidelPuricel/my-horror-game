using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EntityAI : MonoBehaviour 
{
    [Header("Componente")]
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player; 

    [Header("Setări Mișcare")]
    public float walkSpeed = 3.5f;
    public float crawlSpeed = 1.2f;
    public float chaseSpeed = 5.0f;
    public float chaseRange = 15f; 

    [Header("Setări Patrulare")]
    public Transform[] waypoints;
    public float waitTimeAtWaypoint = 2.0f;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    [Header("Setări Detectare (Crawl & Uși)")]
    public float detectionDistance = 2.5f; 
    public float checkHeight = 0.9f; 

    void Start() 
    {
        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update() 
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange) 
        {
            ChasePlayer();
        } 
        else 
        {
            Patrol();
        }

        CheckForDoorAndCrawl();
        UpdateWalkAnimation();
    }

    void Patrol() 
    {
        if (waypoints.Length == 0 || isWaiting) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (!agent.pathPending && agent.remainingDistance < 0.6f) 
        {
            StartCoroutine(StayAtWaypoint());
        }
    }

    void ChasePlayer() 
    {
        isWaiting = false; 
        StopCoroutine("StayAtWaypoint"); 
        agent.isStopped = false;
        agent.SetDestination(player.position);
        
        if (!anim.GetBool("isCrawling"))
            agent.speed = chaseSpeed;
    }

    void CheckForDoorAndCrawl() 
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * checkHeight;
        
        // 1. Detectăm dacă avem un obstacol marcat "Door" în față
        bool wallInFront = Physics.SphereCast(rayOrigin, 0.3f, transform.forward, out hit, detectionDistance);
        Debug.DrawRay(rayOrigin, transform.forward * detectionDistance, Color.red);

        // 2. Detectăm dacă avem tavan jos deasupra capului
        RaycastHit roofHit;
        bool ceilingAbove = Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.up, out roofHit, 1.5f);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.up * 1.5f, Color.blue);

        if ((wallInFront && hit.collider.CompareTag("Door")) || ceilingAbove) 
        {
            anim.SetBool("isCrawling", true);
            agent.speed = crawlSpeed;
            agent.height = 0.8f; 

            // AICI AM MODIFICAT: folosim Toggle() conform scriptului tău Door.cs
            if (wallInFront && hit.collider.CompareTag("Door"))
            {
                var usaScript = hit.collider.GetComponentInParent<Door>();
                if (usaScript != null) usaScript.Toggle(); 
            }
        } 
        else 
        {
            anim.SetBool("isCrawling", false);
            agent.height = 2.0f; 
            
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            agent.speed = (distanceToPlayer <= chaseRange) ? chaseSpeed : walkSpeed;
        }
    }

    void UpdateWalkAnimation() 
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;
        anim.SetBool("isWalking", isMoving);
    }

    IEnumerator StayAtWaypoint() 
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTimeAtWaypoint);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.isStopped = false;
        isWaiting = false;
    }
}
