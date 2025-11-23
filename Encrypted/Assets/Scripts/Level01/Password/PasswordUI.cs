using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PasswordUI : MonoBehaviour
{
    public static PasswordUI Instance { get; private set; }

    [Header("Password Settings")]
    [SerializeField] private string correctPassword ;
    [SerializeField] private string nextSceneName ;

    [Header("UI References")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        passwordPanel.SetActive(false);
        
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(CheckPassword);
        }
        
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(ClosePasswordPanel);
        }
        
        if (passwordInputField != null)
        {
            passwordInputField.onSubmit.AddListener((_) => CheckPassword());
        }
    }

    public void ShowPasswordPanel()
    {
        passwordPanel.SetActive(true);
        passwordInputField.text = "";
        feedbackText.text = "Enter the password:";
        feedbackText.color = Color.white;
        passwordInputField.ActivateInputField();
        
        Time.timeScale = 0f;
        
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.EnableMovement(false);
        }
    }

    public void ClosePasswordPanel()
    {
        passwordPanel.SetActive(false);
        Time.timeScale = 1f;
        
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.EnableMovement(true);
        }
    }

    private void CheckPassword()
{
    string enteredPassword = passwordInputField.text;

    if (enteredPassword == correctPassword)
    {
        feedbackText.text = "Correct! Loading...";
        feedbackText.color = Color.green;
        
        Time.timeScale = 1f;
        LoadNextScene();
    }
    else
    {
        feedbackText.text = "Wrong password!";
        feedbackText.color = Color.red;
        passwordInputField.text = "";
        passwordInputField.ActivateInputField();
    }
}


    private void LoadNextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void SetCorrectPassword(string newPassword)
    {
        correctPassword = newPassword;
    }

    public void SetNextSceneName(string sceneName)
    {
        nextSceneName = sceneName;
    }
}
