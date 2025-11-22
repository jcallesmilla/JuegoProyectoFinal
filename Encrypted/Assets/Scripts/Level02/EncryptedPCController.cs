using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EncryptedPCController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interfazEncryptedCanvas;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private Button secureButton;
    [SerializeField] private Button dangerButton;

    [Header("Button Configuration")]
    [SerializeField] private bool isSecureButtonCorrect = true;

    [Header("Feedback Settings")]
    [SerializeField] private string initialMessage = "classify";
    [SerializeField] private string correctMessage = "Test passed. + Health gained";
    [SerializeField] private string wrongMessage = "ERROR: You have fallen into a phishing trap";
    [SerializeField] private int wrongButtonDamage = 10;
    [SerializeField] private float displayTime = 5f;

    [Header("Feedback Colors")]
    [SerializeField] private Color initialColor = new Color(0.03137255f, 0.9607843f, 0.870588243f, 1f);
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;

    private bool hasBeenUsed = false;
    private Player playerReference;

    private void Start()
    {
        if (interfazEncryptedCanvas != null)
        {
            interfazEncryptedCanvas.SetActive(false);
        }

        if (secureButton != null && dangerButton != null)
        {
            secureButton.onClick.RemoveAllListeners();
            dangerButton.onClick.RemoveAllListeners();

            if (isSecureButtonCorrect)
            {
                secureButton.onClick.AddListener(OnCorrectButtonClicked);
                dangerButton.onClick.AddListener(OnWrongButtonClicked);
            }
            else
            {
                secureButton.onClick.AddListener(OnWrongButtonClicked);
                dangerButton.onClick.AddListener(OnCorrectButtonClicked);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenUsed)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            hasBeenUsed = true;
            playerReference = other.GetComponent<Player>();

            if (playerReference != null)
            {
                ShowCanvas();
                playerReference.EnableMovement(false);
            }
        }
    }

    private void OnCorrectButtonClicked()
    {
        if (feedbackText != null)
        {
            feedbackText.text = correctMessage;
            feedbackText.color = correctColor;
        }

        if (playerReference != null)
        {
            playerReference.currentHealth = playerReference.maxHealth;

            HealthBarPlayer healthBar = FindFirstObjectByType<HealthBarPlayer>();
            if (healthBar != null)
            {
                healthBar.SetHealth(playerReference.currentHealth);
            }
        }

        DisableButtons();
        StartCoroutine(HideCanvasAfterDelay());
    }

    private void OnWrongButtonClicked()
    {
        if (feedbackText != null)
        {
            feedbackText.text = wrongMessage;
            feedbackText.color = wrongColor;
        }

        if (playerReference != null)
        {
            playerReference.BalaDamage(wrongButtonDamage);
        }

        DisableButtons();
        StartCoroutine(HideCanvasAfterDelay());
    }

    private IEnumerator HideCanvasAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        HideCanvas();
    }

    private void ShowCanvas()
    {
        if (interfazEncryptedCanvas != null)
        {
            interfazEncryptedCanvas.SetActive(true);
        }

        if (feedbackText != null)
        {
            feedbackText.text = initialMessage;
            feedbackText.color = initialColor;
        }

        EnableButtons();
    }

    private void HideCanvas()
    {
        if (interfazEncryptedCanvas != null)
        {
            interfazEncryptedCanvas.SetActive(false);
        }

        if (playerReference != null)
        {
            playerReference.EnableMovement(true);
        }
    }

    private void EnableButtons()
    {
        if (secureButton != null)
        {
            secureButton.interactable = true;
        }

        if (dangerButton != null)
        {
            dangerButton.interactable = true;
        }
    }

    private void DisableButtons()
    {
        if (secureButton != null)
        {
            secureButton.interactable = false;
        }

        if (dangerButton != null)
        {
            dangerButton.interactable = false;
        }
    }
}



