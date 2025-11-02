using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseSpeed = 5.0f;
    public float baseJumpForce = 7.0f;
    public int baseMaxHealth = 100;

    [Header("Upgrade Levels")]
    private Dictionary<UpgradeType, int> upgradeLevels = new Dictionary<UpgradeType, int>();

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

    public void UpgradeLevel(UpgradeType type)
    {
        if (!upgradeLevels.ContainsKey(type))
        {
            upgradeLevels[type] = 0;
        }
        upgradeLevels[type]++;
    }

    public float GetCurrentValue(UpgradeType type)
    {
        int level = GetUpgradeLevel(type);

        switch (type)
        {
            case UpgradeType.Speed:
                return baseSpeed + (level * 1f);
            case UpgradeType.Jump:
                return baseJumpForce + (level * 1f);
            case UpgradeType.Health:
                return baseMaxHealth + (level * 20);
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
}
