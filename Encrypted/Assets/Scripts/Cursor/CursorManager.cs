using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D rightClickCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
    [SerializeField][Range(0.5f, 5.0f)] private float cursorScale = 1.0f;

    private Mouse mouse;
    private Texture2D scaledDefaultCursor;
    private Texture2D scaledRightClickCursor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mouse = Mouse.current;
        
        if (cursorScale != 1.0f)
        {
            if (defaultCursor != null)
            {
                scaledDefaultCursor = ScaleTexture(defaultCursor, cursorScale);
            }
            if (rightClickCursor != null)
            {
                scaledRightClickCursor = ScaleTexture(rightClickCursor, cursorScale);
            }
        }
        
        if (rightClickCursor != null)
        {
            SetCursor(scaledRightClickCursor != null ? scaledRightClickCursor : rightClickCursor);
        }
        else
        {
            Debug.LogWarning("[CursorManager] Right click cursor texture is not assigned!");
        }
    }

    private void Update()
    {
        if (mouse == null) return;

        if (mouse.leftButton.isPressed)
        {
            if (defaultCursor != null)
            {
                SetCursor(scaledDefaultCursor != null ? scaledDefaultCursor : defaultCursor);
            }
        }
        else
        {
            if (rightClickCursor != null)
            {
                SetCursor(scaledRightClickCursor != null ? scaledRightClickCursor : rightClickCursor);
            }
        }
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        Vector2 adjustedHotspot = cursorHotspot * cursorScale;
        Cursor.SetCursor(cursorTexture, adjustedHotspot, CursorMode.Auto);
    }

    private Texture2D ScaleTexture(Texture2D source, float scale)
    {
        if (scale == 1.0f) return source;

        int newWidth = Mathf.RoundToInt(source.width * scale);
        int newHeight = Mathf.RoundToInt(source.height * scale);

        Texture2D result = new Texture2D(newWidth, newHeight, source.format, false);
        result.filterMode = FilterMode.Point;

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float u = x / (float)newWidth;
                float v = y / (float)newHeight;
                result.SetPixel(x, y, source.GetPixelBilinear(u, v));
            }
        }

        result.Apply();
        return result;
    }

    private void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        
        if (scaledDefaultCursor != null)
        {
            Destroy(scaledDefaultCursor);
        }
        if (scaledRightClickCursor != null)
        {
            Destroy(scaledRightClickCursor);
        }
    }

    private static CursorManager instance;
}