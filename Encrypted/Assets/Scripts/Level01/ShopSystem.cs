using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    [Header("Upgrade Configuration")]
    [Tooltip("List of all available upgrades")]
    public List<UpgradeData> availableUpgrades = new List<UpgradeData>();

    [Header("UI References")]
    public GameObject shopPanel;
    public Transform upgradeContainer;
    public GameObject upgradeUIPrefab;
    public Button closeButton;
    public TextMeshProUGUI coinDisplayText;

    [Header("Optional Header/Footer")]
    public TextMeshProUGUI shopTitleText;

    private GameManager gameManager;
    private PlayerStats playerStats;
    private List<UpgradeUI> upgradeUIElements = new List<UpgradeUI>();

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerStats = gameManager.playerStats;

        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseShop);
        }

        GenerateUpgradeUI();
    }

    private void Update()
    {
        if (shopPanel != null && shopPanel.activeSelf)
        {
            UpdateShopUI();
        }
    }

    private void GenerateUpgradeUI()
    {
        if (upgradeContainer == null || upgradeUIPrefab == null)
        {
            Debug.LogError("ShopSystem: Missing upgradeContainer or upgradeUIPrefab!");
            return;
        }

        foreach (Transform child in upgradeContainer)
        {
            Destroy(child.gameObject);
        }
        upgradeUIElements.Clear();

        foreach (UpgradeData upgrade in availableUpgrades)
        {
            GameObject uiInstance = Instantiate(upgradeUIPrefab, upgradeContainer);
            UpgradeUI upgradeUI = uiInstance.GetComponent<UpgradeUI>();

            if (upgradeUI != null)
            {
                upgradeUI.Initialize(upgrade, this);
                upgradeUIElements.Add(upgradeUI);
            }
            else
            {
                Debug.LogWarning($"UpgradeUI component not found on prefab for {upgrade.upgradeName}");
            }
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            ForceRebuildLayoutImmediate();
            UpdateShopUI();
            Time.timeScale = 0f;
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void ForceRebuildLayoutImmediate()
    {
        Canvas.ForceUpdateCanvases();
        
        if (shopPanel != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(shopPanel.GetComponent<RectTransform>());
        }
        
        if (upgradeContainer != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(upgradeContainer.GetComponent<RectTransform>());
        }
    }

    private void UpdateShopUI()
    {
        if (gameManager == null) return;

        if (coinDisplayText != null)
        {
            coinDisplayText.text = $"Coins: {gameManager.totalCoins}";
        }

        foreach (UpgradeUI upgradeUI in upgradeUIElements)
        {
            upgradeUI.UpdateDisplay();
        }
    }

    public void PurchaseUpgrade(UpgradeData upgrade)
    {
        int currentLevel = playerStats.GetUpgradeLevel(upgrade.upgradeType);
        int cost = Mathf.RoundToInt(upgrade.baseCost * Mathf.Pow(upgrade.costMultiplier, currentLevel));

        if (currentLevel >= upgrade.maxLevel)
        {
            Debug.Log($"{upgrade.upgradeName} is already at max level!");
            return;
        }

        if (!gameManager.HasEnoughCoins(cost))
        {
            Debug.Log($"Not enough coins to purchase {upgrade.upgradeName}!");
            return;
        }

        gameManager.SpendCoins(cost);
        playerStats.UpgradeLevel(upgrade.upgradeType, upgrade.incrementValue);


        if (upgrade.upgradeType == UpgradeType.Health)
        {
            VidaPlayer vidaPlayer = FindFirstObjectByType<VidaPlayer>();
            if (vidaPlayer != null)
            {
                int newMaxHealth = playerStats.GetCurrentMaxHealth();
                int difference = newMaxHealth - vidaPlayer.maxVida;
                vidaPlayer.maxVida = newMaxHealth;
                vidaPlayer.Curar(difference);
            }
        }

        Debug.Log($"Purchased {upgrade.upgradeName} upgrade! New level: {currentLevel + 1}");
        UpdateShopUI();
    }
}
