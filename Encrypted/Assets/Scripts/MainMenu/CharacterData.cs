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
}
