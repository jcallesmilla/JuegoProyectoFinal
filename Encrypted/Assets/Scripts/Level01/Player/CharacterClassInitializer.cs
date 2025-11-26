using UnityEngine;

public class CharacterClassInitializer : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private const string SELECTED_CLASS_ID_KEY = "SelectedCharacterID";
    
    private void Awake()
    {
        ApplySelectedClassStats();
    }
    
    private void ApplySelectedClassStats()
    {
        int selectedClassID = PlayerPrefs.GetInt(SELECTED_CLASS_ID_KEY, 0);
        
        CharacterData selectedClass = LoadCharacterData(selectedClassID);
        
        if (selectedClass != null)
        {
            ApplyStatsToPlayer(selectedClass);
        }
        else
        {
            Debug.LogWarning($"[CharacterClassInitializer] Could not find class with ID {selectedClassID}. Using default stats.");
        }
    }
    
    private CharacterData LoadCharacterData(int classID)
    {
        CharacterData[] allClasses = Resources.LoadAll<CharacterData>("CharacterClass");
        
        foreach (CharacterData characterClass in allClasses)
        {
            if (characterClass.characterID == classID)
            {
                return characterClass;
            }
        }
        
        if (allClasses.Length > 0)
        {
            return allClasses[0];
        }
        
        return null;
    }
    
    private void ApplyStatsToPlayer(CharacterData characterClass)
    {
        PlayerStats playerStats = GameManager.Instance.playerStats;
        
        if (playerStats != null)
        {
            playerStats.baseSpeed = characterClass.startingSpeed;
            playerStats.baseJumpForce = characterClass.startingJumpForce;
            playerStats.baseMaxHealth = characterClass.startingMaxHealth;
            
            if (showDebugLogs)
            {
                Debug.Log($"[CharacterClassInitializer] ✓ Applied Class: <b>{characterClass.characterName}</b>");
                Debug.Log($"  → Speed: {characterClass.startingSpeed}");
                Debug.Log($"  → Jump: {characterClass.startingJumpForce}");
                Debug.Log($"  → Health: {characterClass.startingMaxHealth}");
            }
        }
        else
        {
            Debug.LogError("[CharacterClassInitializer] PlayerStats not found in GameManager!");
        }
    }
}

