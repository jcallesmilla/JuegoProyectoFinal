using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Select/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public int characterID;
    
    [Header("Visual Only (Cosmetic)")]
    public Sprite characterIcon;
    public Sprite characterPreviewImage;
    public RuntimeAnimatorController animatorController;
    
    [Header("Description")]
    [TextArea(3, 5)]
    public string description;
    
    [Header("Starting Stats")]
    public float startingSpeed = 5.0f;
    public float startingJumpForce = 15.0f;
    public int startingMaxHealth = 100;
    
    [Header("Stat Display (Visual Rating)")]
    [Tooltip("Visual rating for UI - 1 to 5 stars")]
    [Range(1, 5)] public int healthRating = 3;
    [Range(1, 5)] public int speedRating = 3;
    [Range(1, 5)] public int jumpRating = 3;
}
