using UnityEngine;
using TMPro;

public class ShopTerminal : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Key to press to open shop")]
    public KeyCode interactionKey = KeyCode.E;
    [Tooltip("Show prompt when player is nearby")]
    public bool showPrompt = true;

    [Header("Prompt UI")]
    [Tooltip("Text shown above terminal (e.g., 'Press E to Shop')")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;
    public string promptMessage = "Press E to Shop";
    public float promptOffsetY = 1.5f;

    [Header("Shop Reference")]
    [Tooltip("The ShopSystem component (usually on a child Canvas)")]
    public ShopSystem shopSystem;

    private bool playerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        if (shopSystem == null)
        {
            shopSystem = GetComponentInChildren<ShopSystem>();
        }

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        if (promptText != null)
        {
            promptText.text = promptMessage;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            OpenShop();
        }

        if (promptUI != null && playerInRange && playerTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * promptOffsetY);
            promptUI.transform.position = screenPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;

            if (showPrompt && promptUI != null)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;

            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }

    private void OpenShop()
    {
        if (shopSystem != null)
        {
            shopSystem.OpenShop();
            
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("ShopTerminal: No ShopSystem found!");
        }
    }
}
