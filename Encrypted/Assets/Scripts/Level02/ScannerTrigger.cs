using UnityEngine;

public class ScannerTrigger : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject warningCanvas;
    public GameObject pantallaRojaCanvas;
    
    [Header("Blink Settings")]
    public float blinkInterval = 0.5f;
    
    private bool triggered = false;
    private Player player;
    private bool isBlinking = false;
    private float blinkTimer = 0f;
    private bool isPantallaRojaVisible = false;

    void Start()
    {
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
            
        if (pantallaRojaCanvas != null)
            pantallaRojaCanvas.SetActive(false);
    }

    void Update()
    {
        if (triggered && (player == null || player.currentHealth <= 0))
        {
            StopBlinking();
        }
        else if (isBlinking)
        {
            HandleBlinking();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            player = other.GetComponent<Player>();
            
            if (player != null)
            {
                triggered = true;
                
                if (warningCanvas != null)
                    warningCanvas.SetActive(true);
                    
                StartBlinking();
            }
        }
    }
    
    private void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f;
        
        if (pantallaRojaCanvas != null)
        {
            pantallaRojaCanvas.SetActive(true);
            isPantallaRojaVisible = true;
        }
    }
    
    private void StopBlinking()
    {
        isBlinking = false;
        
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
            
        if (pantallaRojaCanvas != null)
            pantallaRojaCanvas.SetActive(false);
    }
    
    private void HandleBlinking()
    {
        blinkTimer += Time.deltaTime;
        
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            isPantallaRojaVisible = !isPantallaRojaVisible;
            
            if (pantallaRojaCanvas != null)
                pantallaRojaCanvas.SetActive(isPantallaRojaVisible);
        }
    }
}









