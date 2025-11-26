using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public PlayerStats playerStats;
    public int totalCoins = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeComponents();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeComponents()
    {
        if (playerStats == null)
        {
            playerStats = gameObject.AddComponent<PlayerStats>();
        }
        
        if (GetComponent<CharacterClassInitializer>() == null)
        {
            gameObject.AddComponent<CharacterClassInitializer>();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SyncCoinsToManager();
        SyncHealthToPlayer();
    }

    private void SyncCoinsToManager()
    {
        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.coinCount = totalCoins;
        }
    }

    private void SyncHealthToPlayer()
    {
        VidaPlayer vidaPlayer = FindFirstObjectByType<VidaPlayer>();
        if (vidaPlayer != null && playerStats != null)
        {
            vidaPlayer.maxVida = playerStats.GetCurrentMaxHealth();
            vidaPlayer.cantidadDeVida = Mathf.Min(vidaPlayer.cantidadDeVida, vidaPlayer.maxVida);
        }
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.coinCount = totalCoins;
        }
    }

    public void SpendCoins(int amount)
    {
        totalCoins -= amount;
        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.coinCount = totalCoins;
        }
    }

    public bool HasEnoughCoins(int amount)
    {
        return totalCoins >= amount;
    }
    
    public void ResetGameProgress()
    {
        totalCoins = 0;

        if (playerStats != null)
        {
            playerStats.ResetAllUpgrades();
        }

        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.coinCount = 0;
        }
    }
}
