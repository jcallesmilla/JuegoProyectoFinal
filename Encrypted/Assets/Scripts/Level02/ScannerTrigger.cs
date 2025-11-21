using UnityEngine;

public class ScannerTrigger : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject warningCanvas;
    public GameObject pantallaRojaCanvas;
    
    [Header("Flashing Settings")]
    public float flashInterval = 3f;
    
    private bool triggered = false;
    private float timer = 0f;
    private bool isPantallaRojaVisible = false;
    private Player player;

    void Start()
    {
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
            
        if (pantallaRojaCanvas != null)
            pantallaRojaCanvas.SetActive(false);
    }

    void Update()
    {
        if (triggered && player != null && player.currentHealth > 0)
        {
            timer += Time.deltaTime;
            
            if (timer >= flashInterval)
            {
                isPantallaRojaVisible = !isPantallaRojaVisible;
                pantallaRojaCanvas.SetActive(isPantallaRojaVisible);
                timer = 0f;
            }
        }
        else if (triggered && (player == null || player.currentHealth <= 0))
        {
            if (warningCanvas != null)
                warningCanvas.SetActive(false);
                
            if (pantallaRojaCanvas != null)
                pantallaRojaCanvas.SetActive(false);
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
            }
        }
    }
}




