using UnityEngine;
using System.Collections.Generic;

public class ScannerTrigger : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject warningCanvas;
    public GameObject pantallaRojaCanvas;
    
    [Header("Blink Settings")]
    public float blinkInterval = 0.5f;
    
    [Header("FlyDrone Settings")]
    [SerializeField] private GameObject flyDronePrefab;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float minSpawnDistance = 10f;
    [SerializeField] private float maxSpawnDistance = 15f;
    
    private bool triggered = false;
    private Player player;
    private bool isBlinking = false;
    private float blinkTimer = 0f;
    private bool isPantallaRojaVisible = false;
    private List<FlyDrone> spawnedDrones = new List<FlyDrone>();
    private bool dronesActive = false;
    private Camera mainCamera;

    void Start()
    {
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
            
        if (pantallaRojaCanvas != null)
            pantallaRojaCanvas.SetActive(false);

        mainCamera = Camera.main;
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

        if (dronesActive)
        {
            CheckDronesStatus();
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
                SpawnFlyDrones();
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

    private void SpawnFlyDrones()
    {
        if (flyDronePrefab == null || player == null)
        {
            Debug.LogWarning("FlyDrone prefab or Player is missing!");
            return;
        }

        spawnedDrones.Clear();

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject droneObject = Instantiate(flyDronePrefab, spawnPosition, Quaternion.identity);
            FlyDrone drone = droneObject.GetComponent<FlyDrone>();
            
            if (drone != null)
            {
                spawnedDrones.Add(drone);
            }
        }

        dronesActive = true;
        FlyDrone.OnDroneDestroyed += OnDroneDestroyed;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        if (mainCamera == null || player == null)
        {
            return player.transform.position + (Vector3)Random.insideUnitCircle * minSpawnDistance;
        }

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector2 cameraCenter = mainCamera.transform.position;
        Vector2 playerPosition = player.transform.position;

        Vector2 spawnPosition;
        int attempts = 0;
        int maxAttempts = 20;

        do
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            
            spawnPosition = playerPosition + new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );

            attempts++;
            
            if (attempts >= maxAttempts)
            {
                break;
            }

        } while (IsPositionInCameraView(spawnPosition, cameraCenter, cameraWidth, cameraHeight));

        return spawnPosition;
    }

    private bool IsPositionInCameraView(Vector2 position, Vector2 cameraCenter, float cameraWidth, float cameraHeight)
    {
        float halfWidth = cameraWidth / 2f;
        float halfHeight = cameraHeight / 2f;

        return position.x >= cameraCenter.x - halfWidth &&
               position.x <= cameraCenter.x + halfWidth &&
               position.y >= cameraCenter.y - halfHeight &&
               position.y <= cameraCenter.y + halfHeight;
    }

    private void OnDroneDestroyed(FlyDrone drone)
    {
        if (spawnedDrones.Contains(drone))
        {
            spawnedDrones.Remove(drone);
        }
    }

    private void CheckDronesStatus()
    {
        spawnedDrones.RemoveAll(drone => drone == null);

        if (spawnedDrones.Count == 0)
        {
            dronesActive = false;
            ResetTrigger();
        }
    }

    private void ResetTrigger()
    {
        triggered = false;
        StopBlinking();
        FlyDrone.OnDroneDestroyed -= OnDroneDestroyed;
    }

    private void OnDestroy()
    {
        FlyDrone.OnDroneDestroyed -= OnDroneDestroyed;
    }
}











