using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Character Database")]
    [SerializeField] private CharacterData[] availableCharacters;
    
    [Header("UI References")]
    [SerializeField] private Image characterPreviewImage;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterDescriptionText;
    [SerializeField] private TextMeshProUGUI characterCounterText;
    
    [Header("Navigation Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button backButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Level01";
    
    private CharacterLinkedList.DoublyLinkedList characterList;
    private CharacterLinkedList.Node currentNode;

    private void Start()
    {
        InitializeCharacterList();
        SetupButtons();
        UpdateUI();
    }

    private void InitializeCharacterList()
    {
        characterList = new CharacterLinkedList.DoublyLinkedList();
        
        foreach (CharacterData character in availableCharacters)
        {
            if (character != null)
            {
                characterList.InsertAtEnd(character);
            }
        }
        
        if (characterList.GetCount() == 0)
        {
            Debug.LogError("No characters available in the character select!");
            return;
        }

        int savedCharacterID = PlayerPrefs.GetInt("SelectedCharacterID", 0);
        CharacterLinkedList.Node savedNode = characterList.FindByID(savedCharacterID);
        
        if (savedNode != null)
        {
            currentNode = savedNode;
        }
        else
        {
            currentNode = characterList.GetCurrentNode();
        }
    }

    private void SetupButtons()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextCharacter);
        
        if (previousButton != null)
            previousButton.onClick.AddListener(OnPreviousCharacter);
        
        if (selectButton != null)
            selectButton.onClick.AddListener(OnSelectCharacter);
        
        if (backButton != null)
            backButton.onClick.AddListener(OnBack);
    }

    private void OnNextCharacter()
    {
        if (currentNode != null)
        {
            currentNode = characterList.GetNextNode(currentNode);
            UpdateUI();
        }
    }

    private void OnPreviousCharacter()
    {
        if (currentNode != null)
        {
            currentNode = characterList.GetPreviousNode(currentNode);
            UpdateUI();
        }
    }

private void OnSelectCharacter()
{
    if (currentNode != null && currentNode.item != null)
    {
        CharacterData selectedCharacter = currentNode.item;
        
        PlayerPrefs.SetInt("SelectedCharacterID", selectedCharacter.characterID);
        PlayerPrefs.SetString("SelectedCharacterName", selectedCharacter.characterName);
        
        PlayerPrefs.Save();
        
        Debug.Log($"✓ Class selected: {selectedCharacter.characterName} (ID: {selectedCharacter.characterID})");
        
        if (characterNameText != null)
        {
            characterNameText.text = $"✓ {selectedCharacter.characterName} Selected!";
        }
        
        if (selectButton != null)
        {
            TextMeshProUGUI buttonText = selectButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Selected!";
            }
        }
    }
}



        
    private void OnBack()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void UpdateUI()
{
    if (currentNode == null || currentNode.item == null) return;

    CharacterData currentCharacter = currentNode.item;

    if (characterPreviewImage != null && currentCharacter.characterPreviewImage != null)
    {
        characterPreviewImage.sprite = currentCharacter.characterPreviewImage;
    }
    
    if (characterNameText != null)
    {
        characterNameText.text = currentCharacter.characterName;
    }
    
    if (characterDescriptionText != null)
    {
        string displayText = currentCharacter.description + "\n\n";
        displayText += "<b>STARTING STATS:</b>\n";
        displayText += $"Health: {currentCharacter.startingMaxHealth} {GetStarRating(currentCharacter.healthRating)}\n";
        displayText += $"Speed: {currentCharacter.startingSpeed:F1} {GetStarRating(currentCharacter.speedRating)}\n";
        displayText += $"Jump: {currentCharacter.startingJumpForce:F1} {GetStarRating(currentCharacter.jumpRating)}";
        
        characterDescriptionText.text = displayText;
    }
    
    if (characterCounterText != null)
    {
        int currentIndex = GetCurrentIndex() + 1;
        int totalCount = characterList.GetCount();
        characterCounterText.text = $"{currentIndex} / {totalCount}";
    }
}

private string GetStarRating(int rating)
{
    string stars = "";
    for (int i = 0; i < 5; i++)
    {
        stars += i < rating ? "★" : "☆";
    }
    return stars;
}


    private int GetCurrentIndex()
    {
        if (currentNode == null || currentNode.item == null) return 0;
        
        for (int i = 0; i < availableCharacters.Length; i++)
        {
            if (availableCharacters[i] == currentNode.item)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            OnNextCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            OnPreviousCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnSelectCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
    }
}
