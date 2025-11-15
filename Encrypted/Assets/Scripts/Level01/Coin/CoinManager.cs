using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coinText;

    [Header("UI Position")]
    [Tooltip("Margin in pixels from the top-left corner")]
    public Vector2 margin = new Vector2(10f, 10f);

    [Header("Text settings")]
    [Tooltip("Font size for the coin text (editable from Inspector)")]
    public float fontSize = 24f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        coinCount = gameManager.totalCoins;

        if (coinText != null)
        {
            RectTransform rt = coinText.rectTransform;
            rt.anchorMin = new Vector2(1f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(-margin.x, -margin.y);
            coinText.fontSize = fontSize;
        }
        else
        {
            Debug.LogWarning("CoinManager: 'coinText' is not assigned in the Inspector.");
        }
    }

    void Update() //se elimina update y lateupdate anterior ya que es puramente display el coinmanager y se hace todo desde gamemanager
    {
        if (coinText == null || gameManager == null) return;

        if (Mathf.Abs(coinText.fontSize - fontSize) > 0.01f)
            coinText.fontSize = fontSize;

        coinCount = gameManager.totalCoins;
        coinText.text = "Coin count: " + coinCount.ToString();
    }

}

