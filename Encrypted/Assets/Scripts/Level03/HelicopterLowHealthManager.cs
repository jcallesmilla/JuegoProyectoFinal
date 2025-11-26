using UnityEngine;

public class HelicopterLowHealthManager : MonoBehaviour
{
    [Header("Canvas Reference")]
    public GameObject lowHealthCanvas;
    
    [Header("Health Settings")]
    [Tooltip("Use percentage (0-100) of max health as threshold")]
    public bool usePercentage = false;
    [Tooltip("If usePercentage is false, this is the absolute health value threshold")]
    public float lowHealthThreshold = 6f;
    [Tooltip("If usePercentage is true, trigger when health is below this percentage")]
    [Range(0, 100)]
    public float lowHealthPercentage = 30f;
    
    [Header("Blink Settings")]
    public float blinkInterval = 0.5f;
    
    private HelicopterController helicopter;
    private bool isBlinking = false;
    private float blinkTimer = 0f;
    private bool isCanvasVisible = false;

    void Start()
    {
        helicopter = FindFirstObjectByType<HelicopterController>();
        
        if (lowHealthCanvas != null)
        {
            lowHealthCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (helicopter == null) return;

        float currentThreshold = GetCurrentThreshold();
        
        if (helicopter.currentHealth <= currentThreshold && helicopter.currentHealth > 0)
        {
            if (!isBlinking)
            {
                StartBlinking();
            }
            HandleBlinking();
        }
        else
        {
            if (isBlinking)
            {
                StopBlinking();
            }
        }
    }

    private float GetCurrentThreshold()
    {
        if (usePercentage)
        {
            int maxHealth = helicopter.maxHealth;
            return maxHealth * (lowHealthPercentage / 100f);
        }
        else
        {
            return lowHealthThreshold;
        }
    }

    private void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f;
        
        if (lowHealthCanvas != null)
        {
            lowHealthCanvas.SetActive(true);
            isCanvasVisible = true;
        }
    }

    private void StopBlinking()
    {
        isBlinking = false;
        
        if (lowHealthCanvas != null)
        {
            lowHealthCanvas.SetActive(false);
        }
    }

    private void HandleBlinking()
    {
        blinkTimer += Time.deltaTime;
        
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            isCanvasVisible = !isCanvasVisible;
            
            if (lowHealthCanvas != null)
            {
                lowHealthCanvas.SetActive(isCanvasVisible);
            }
        }
    }
}

