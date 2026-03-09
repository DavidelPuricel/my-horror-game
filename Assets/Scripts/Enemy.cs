using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public Transform player;
	public int damage;
	public float detectionRange;
	public float hitRange;
	public float cooldown;
	
	private bool HitTaken=false;
	private NavMeshAgent agent;
	private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private IEnumerator HitCooldownCoroutine()
    {
    	HitTaken=true;
    	agent.isStopped=true;
    	yield return new WaitForSeconds(cooldown);
    	HitTaken=false;
    	agent.isStopped = false;
    }
    	

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance<hitRange)
        {
        if(!HitTaken&&playerHealth!=null)
        {
        playerHealth.TakeDamage(damage);
        Debug.Log("Player Hit. Damage: " + damage + " | Player Health: " + playerHealth.GetCurrentHealth());
        StartCoroutine(HitCooldownCoroutine());
        }
        }
        else if (distance<detectionRange)
        {
        agent.SetDestination(player.position);
        }
        else
        {
        agent.SetDestination(transform.position);
        }
    }
}
