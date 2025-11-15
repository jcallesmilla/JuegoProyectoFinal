using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Shop/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Display Info")]
    public string upgradeName = "Speed";
    public string description = "Increases movement speed";
    public Sprite icon;

    [Header("Cost Settings")]
    public int baseCost = 10;
    public float costMultiplier = 1.5f;

    [Header("Upgrade Values")]
    public UpgradeType upgradeType;
    public float incrementValue = 1f;
    public int maxLevel = 5;

    [Header("Display Format")]
    public string statFormat = "{0:F1}";
    public string statLabel = "Speed";
}

public enum UpgradeType
{
    Speed,
    Jump,
    Health,
    Damage,
    Defense,
    AttackSpeed,
    CritChance,
    MaxEnergy
}

