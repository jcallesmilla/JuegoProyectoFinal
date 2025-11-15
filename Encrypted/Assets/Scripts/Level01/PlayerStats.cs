using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseSpeed = 5.0f;
    public float baseJumpForce = 15.0f;
    public int baseMaxHealth = 100;

    [Header("Upgrade Levels")]
    private Dictionary<UpgradeType, int> upgradeLevels = new Dictionary<UpgradeType, int>();
    private Dictionary<UpgradeType, float> upgradeIncrements = new Dictionary<UpgradeType, float>();

    private void Awake()
    {
        InitializeUpgradeLevels();
    }

    private void InitializeUpgradeLevels()
    {
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            if (!upgradeLevels.ContainsKey(type))
            {
                upgradeLevels[type] = 0;
            }
            if (!upgradeIncrements.ContainsKey(type))
            {
                upgradeIncrements[type] = 0f;
            }
        }
    }

    public int GetUpgradeLevel(UpgradeType type)
    {
        if (!upgradeLevels.ContainsKey(type))
        {
            upgradeLevels[type] = 0;
        }
        return upgradeLevels[type];
    }

    public void UpgradeLevel(UpgradeType type, float incrementValue)
    {
        if (!upgradeLevels.ContainsKey(type))
        {
            upgradeLevels[type] = 0;
        }
        upgradeLevels[type]++;
        upgradeIncrements[type] = incrementValue;
    }

    public float GetCurrentValue(UpgradeType type)
    {
        int level = GetUpgradeLevel(type);
        float increment = upgradeIncrements.ContainsKey(type) ? upgradeIncrements[type] : 0f;

        switch (type)
        {
            case UpgradeType.Speed:
                return baseSpeed + (level * increment);
            case UpgradeType.Jump:
                return baseJumpForce + (level * increment);
            case UpgradeType.Health:
                return baseMaxHealth + (level * increment);
            default:
                return 0f;
        }
    }

    public float GetCurrentSpeed()
    {
        return GetCurrentValue(UpgradeType.Speed);
    }

    public float GetCurrentJumpForce()
    {
        return GetCurrentValue(UpgradeType.Jump);
    }

    public int GetCurrentMaxHealth()
    {
        return Mathf.RoundToInt(GetCurrentValue(UpgradeType.Health));
    }
     public void ResetAllUpgrades()
    {
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            upgradeLevels[type] = 0;
            upgradeIncrements[type] = 0f;
        }
        Debug.Log("All upgrades have been reset!");
    }
}

