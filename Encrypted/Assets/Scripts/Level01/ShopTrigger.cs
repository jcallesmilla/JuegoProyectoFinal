using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool requireInteraction = true;
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI")]
    public GameObject promptUI;

    private bool playerInRange = false;
    private ShopSystem shopSystem;

    private void Start()
    {
        shopSystem = FindFirstObjectByType<ShopSystem>();
        
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && requireInteraction)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                OpenShop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (promptUI != null)
            {
                promptUI.SetActive(true);
            }

            if (!requireInteraction)
            {
                OpenShop();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
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
        }
    }
}
