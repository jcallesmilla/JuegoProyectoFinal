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


    // referencias internas a las imágenes del HUD
    private Image barraFill; // imagen verde (fill)
    private RectTransform barraFillRect; // rect transform of the green fill (we'll resize width)

    void Start()
    {
        // Clamp initial vida
        cantidadDeVida = Mathf.Clamp(cantidadDeVida, 0, maxVida);

        CreateOrUseHUD();
        UpdateBar();
    }

    // Public helper para recibir daño
    public void QuitarVida(int cantidad)
    {
        cantidadDeVida -= cantidad;
        Debug.Log($"QuitarVida called: -{cantidad}, nueva vida={cantidadDeVida}");
        if (cantidadDeVida <= 0)
        {
            cantidadDeVida = 0;
            UpdateBar();
            // destruir el GameObject del jugador cuando la vida llegue a 0
            Destroy(gameObject);
            return;
        }

        UpdateBar();
    }

    // Public helper para curar
    public void Curar(int cantidad)
    {
        cantidadDeVida = Mathf.Min(maxVida, cantidadDeVida + cantidad);
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (barraFill == null) return;
        float fill = (float)cantidadDeVida / (float)maxVida;
        // Prefer resizing the rect so the background (red) is always visible behind
        if (barraFillRect != null)
        {
            float width = barraSize.x * fill;
            barraFillRect.sizeDelta = new Vector2(width, barraFillRect.sizeDelta.y);
        }
        else
        {
            // fallback to Image.Fill if present
            barraFill.type = Image.Type.Filled;
            barraFill.fillAmount = fill;
        }
        Debug.Log($"UpdateBar: fill={fill} ({cantidadDeVida}/{maxVida})");
    }

    // Crea un Canvas/HUD si no existe y una barra fija en la esquina izquierda superior
    private void CreateOrUseHUD()
    {
        // Try to find an existing HUD canvas named "HUD_Canvas"
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
            // create a new canvas
            GameObject canvasGO = new GameObject("HUD_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

    // Create panel/background for the bar
    GameObject panel = new GameObject("VidaPanel");
    panel.transform.SetParent(canvas.transform, false);
    RectTransform rt = panel.AddComponent<RectTransform>();
    // Anchor top-left
    rt.anchorMin = new Vector2(0f, 1f);
    rt.anchorMax = new Vector2(0f, 1f);
    rt.pivot = new Vector2(0f, 1f);
    rt.anchoredPosition = new Vector2(margen.x, -margen.y);
    rt.sizeDelta = barraSize;

    Image bg = panel.AddComponent<Image>();
    // Use the panel background as the "lost" red area so it is visible behind the green fill
    bg.color = barraPerdidaColor;

    // Create fill image as child (green) on top of the red background
    GameObject fill = new GameObject("VidaFill");
    fill.transform.SetParent(panel.transform, false);
    RectTransform frt = fill.AddComponent<RectTransform>();
    // Anchor to the left and control width via sizeDelta so the red bg is visible on the right
    frt.anchorMin = new Vector2(0f, 0f);
    frt.anchorMax = new Vector2(0f, 1f);
    frt.pivot = new Vector2(0f, 0.5f);
    float initialFill = (float)cantidadDeVida / (float)maxVida;
    frt.sizeDelta = new Vector2(barraSize.x * initialFill, 0f);
    frt.anchoredPosition = new Vector2(0f, 0f);

    barraFillRect = frt;
    barraFill = fill.AddComponent<Image>();
    barraFill.color = barraColor;
    // keep simple image type; we control width directly
    barraFill.type = Image.Type.Simple;

    // ensure correct draw order: background (red) behind, fill (green) above, text on top
    bg.transform.SetSiblingIndex(0);
    fill.transform.SetSiblingIndex(1);

    // No numeric text: user requested to remove life-number display
    }
}
