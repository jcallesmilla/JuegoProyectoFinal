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

    void Start()
    {
        // If coinText is assigned, position it in the top-left of the screen
        if (coinText != null)
        {
            RectTransform rt = coinText.rectTransform;
            // Anchor to top-right
            rt.anchorMin = new Vector2(1f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            // Set anchored position using margin: negative x moves left from right edge, negative y moves down
            rt.anchoredPosition = new Vector2(-margin.x, -margin.y);
            // Apply font size
            coinText.fontSize = fontSize;
        }
        else
        {
            Debug.LogWarning("CoinManager: 'coinText' is not assigned in the Inspector.");
        }

    } 

    void Update()
    {
        if (coinText == null) return;

        // Apply font size if changed in Inspector at runtime
        if (Mathf.Abs(coinText.fontSize - fontSize) > 0.01f)
            coinText.fontSize = fontSize;

        coinText.text = "Coin count: " + coinCount.ToString();
    }
    
    
}
