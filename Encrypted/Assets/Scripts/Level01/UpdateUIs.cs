using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentStatText;
    public TextMeshProUGUI nextStatText;
    public Image iconImage;
    public Button buyButton;

    private UpgradeData upgradeData;
    private ShopSystem shopSystem;
    private int currentLevel;

    public void Initialize(UpgradeData data, ShopSystem shop)
    {
        upgradeData = data;
        shopSystem = shop;

        if (nameText != null)
            nameText.text = data.upgradeName;

        if (descriptionText != null)
            descriptionText.text = data.description;

        if (iconImage != null && data.icon != null)
            iconImage.sprite = data.icon;

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (upgradeData == null || shopSystem == null) return;

        GameManager gameManager = GameManager.Instance;
        PlayerStats playerStats = gameManager.playerStats;

        currentLevel = playerStats.GetUpgradeLevel(upgradeData.upgradeType);
        int cost = GetCurrentCost();
        bool canUpgrade = currentLevel < upgradeData.maxLevel;
        bool canAfford = gameManager.HasEnoughCoins(cost);

        if (costText != null)
        {
            costText.text = canUpgrade ? $"Cost: {cost}" : "MAX";
        }

        if (levelText != null)
        {
            levelText.text = $"Level: {currentLevel}/{upgradeData.maxLevel}";
        }

        if (currentStatText != null)
        {
            float currentValue = playerStats.GetCurrentValue(upgradeData.upgradeType);
            currentStatText.text = $"{upgradeData.statLabel}: {string.Format(upgradeData.statFormat, currentValue)}";
        }

        if (nextStatText != null && canUpgrade)
        {
            float nextValue = playerStats.GetCurrentValue(upgradeData.upgradeType) + upgradeData.incrementValue;
            nextStatText.text = $"→ {string.Format(upgradeData.statFormat, nextValue)}";
            nextStatText.gameObject.SetActive(true);
        }
        else if (nextStatText != null)
        {
            nextStatText.gameObject.SetActive(false);
        }

        if (buyButton != null)
        {
            buyButton.interactable = canUpgrade && canAfford;
        }
    }

    private int GetCurrentCost()
    {
        return Mathf.RoundToInt(upgradeData.baseCost * Mathf.Pow(upgradeData.costMultiplier, currentLevel));
    }

    private void OnBuyClicked()
    {
        if (shopSystem != null)
        {
            shopSystem.PurchaseUpgrade(upgradeData);
        }
    }
}

