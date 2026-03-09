using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCoins playerCoins = other.GetComponent<PlayerCoins>();

            if (playerCoins != null)
            {
                playerCoins.AddCoin(coinValue);
            }

            Destroy(gameObject);
        }
    }
}
