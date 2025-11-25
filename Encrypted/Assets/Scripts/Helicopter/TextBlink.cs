using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private float blinkSpeed = 1f;
    [SerializeField] private bool useAlphaFade = true;
    
    private TextMeshProUGUI textMesh;
    private float blinkTimer = 0f;
    
    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        blinkTimer += Time.deltaTime * blinkSpeed;
        
        if (useAlphaFade)
        {
            float alpha = Mathf.PingPong(blinkTimer, 1f);
            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
        else
        {
            textMesh.enabled = Mathf.PingPong(blinkTimer, 1f) > 0.5f;
        }
    }
}

