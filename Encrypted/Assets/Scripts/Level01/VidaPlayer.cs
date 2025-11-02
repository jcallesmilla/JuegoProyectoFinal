using UnityEngine;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{
    [Header("Vida settings")]
    [Tooltip("Cantidad máxima de vida del jugador")]
    public int maxVida = 100;
    [Tooltip("Vida actual del jugador")]
    public int cantidadDeVida = 100;

    [Header("HUD (barra de vida)")]
    [Tooltip("Tamaño (px) de la barra de vida")]
    public Vector2 barraSize = new Vector2(200f, 24f);
    [Tooltip("Margen desde la esquina superior izquierda (x,y)")]
    public Vector2 margen = new Vector2(10f, 10f);
    [Tooltip("Color de la barra de vida (porción llena)")]
    public Color barraColor = Color.green;
    [Tooltip("Color de la porción perdida (se mostrará en rojo por defecto)")]
    public Color barraPerdidaColor = Color.red;

    private Image barraFill;
    private RectTransform barraFillRect;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GameManager.Instance.playerStats;
        
        if (playerStats != null)
        {
            maxVida = playerStats.GetCurrentMaxHealth();
        }
        
        cantidadDeVida = Mathf.Clamp(cantidadDeVida, 0, maxVida);

        CreateOrUseHUD();
        UpdateBar();
    }

    void Update()
    {
        if (playerStats != null)
        {
            int newMaxVida = playerStats.GetCurrentMaxHealth();
            if (newMaxVida != maxVida)
            {
                maxVida = newMaxVida;
                UpdateBar();
            }
        }
    }

    public void QuitarVida(int cantidad)
    {
        cantidadDeVida -= cantidad;
        Debug.Log($"QuitarVida called: -{cantidad}, nueva vida={cantidadDeVida}");
        if (cantidadDeVida <= 0)
        {
            cantidadDeVida = 0;
            UpdateBar();
            Destroy(gameObject);
            return;
        }

        UpdateBar();
    }

    public void Curar(int cantidad)
    {
        cantidadDeVida = Mathf.Min(maxVida, cantidadDeVida + cantidad);
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (barraFill == null) return;
        float fill = (float)cantidadDeVida / (float)maxVida;
        if (barraFillRect != null)
        {
            float width = barraSize.x * fill;
            barraFillRect.sizeDelta = new Vector2(width, barraFillRect.sizeDelta.y);
        }
        else
        {
            barraFill.type = Image.Type.Filled;
            barraFill.fillAmount = fill;
        }
        Debug.Log($"UpdateBar: fill={fill} ({cantidadDeVida}/{maxVida})");
    }

    private void CreateOrUseHUD()
    {
        Canvas canvas = null;
        Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var c in canvases)
        {
            if (c.gameObject.name == "HUD_Canvas")
            {
                canvas = c;
                break;
            }
        }

        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("HUD_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        GameObject panel = new GameObject("VidaPanel");
        panel.transform.SetParent(canvas.transform, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(margen.x, -margen.y);
        rt.sizeDelta = barraSize;

        Image bg = panel.AddComponent<Image>();
        bg.color = barraPerdidaColor;

        GameObject fill = new GameObject("VidaFill");
        fill.transform.SetParent(panel.transform, false);
        RectTransform frt = fill.AddComponent<RectTransform>();
        frt.anchorMin = new Vector2(0f, 0f);
        frt.anchorMax = new Vector2(0f, 1f);
        frt.pivot = new Vector2(0f, 0.5f);
        float initialFill = (float)cantidadDeVida / (float)maxVida;
        frt.sizeDelta = new Vector2(barraSize.x * initialFill, 0f);
        frt.anchoredPosition = new Vector2(0f, 0f);

        barraFillRect = frt;
        barraFill = fill.AddComponent<Image>();
        barraFill.color = barraColor;
        barraFill.type = Image.Type.Simple;

        bg.transform.SetSiblingIndex(0);
        fill.transform.SetSiblingIndex(1);
    }
}
