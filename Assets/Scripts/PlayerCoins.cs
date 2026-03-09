using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public int coins = 0;
    public GameUI gameUI;

    void Start()
    {
        if (gameUI != null)
            gameUI.UpdateCoins(coins);
    }

    public void AddCoin(int amount)
    {
        coins += amount;

        if (gameUI != null)
            gameUI.UpdateCoins(coins);
    }
}
