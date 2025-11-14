using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("Character Database")]
    [SerializeField] private CharacterData[] characterDatabase;
    
    private Animator playerAnimator;
    private int selectedCharacterID;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        LoadSelectedCharacterSkin();
    }

    private void LoadSelectedCharacterSkin()
    {
        selectedCharacterID = PlayerPrefs.GetInt("SelectedCharacterID", 0);
        
        CharacterData selectedCharacter = GetCharacterByID(selectedCharacterID);
        
        if (selectedCharacter != null && playerAnimator != null)
        {
            if (selectedCharacter.animatorController != null)
            {
                playerAnimator.runtimeAnimatorController = selectedCharacter.animatorController;
                Debug.Log($"Applied character skin: {selectedCharacter.characterName}");
            }
        }
        else
        {
            Debug.LogWarning("No character selected or animator not found. Using default skin.");
        }
    }

    private CharacterData GetCharacterByID(int id)
    {
        foreach (CharacterData character in characterDatabase)
        {
            if (character != null && character.characterID == id)
            {
                return character;
            }
        }
        return null;
    }
}
