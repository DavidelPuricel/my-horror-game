using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text coinsText;
    public Slider playerHealthSlider;

    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coins;
    }

    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = maxHealth;
            playerHealthSlider.value = currentHealth;
        }
    }
}
