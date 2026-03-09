using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Transform respawnPoint;
    public float invincibilityTime = 0.7f;
    public GameUI gameUI;

    private  int currentHealth=50;
    private bool isInvincible = false;
    private Vector3 startPosition;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position;
        currentHealth = maxHealth;
        
        if(gameUI != null)
        {
        gameUI.UpdatePlayerHealth(currentHealth, maxHealth);
        }
    }
    public int GetCurrentHealth()
    {
    	return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        if(gameUI!=null)
        {
        gameUI.UpdatePlayerHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Respawn();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        
        if(gameUI!=null)
        {
        gameUI.UpdatePlayerHealth(currentHealth, maxHealth);
        }

        Vector3 targetPosition = respawnPoint != null ? respawnPoint.position : startPosition;

        if (controller != null)
            controller.enabled = false;

        transform.position = targetPosition;

        if (controller != null)
            controller.enabled = true;
    }
}
