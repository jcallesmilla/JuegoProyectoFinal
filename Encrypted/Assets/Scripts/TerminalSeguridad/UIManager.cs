using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject panelPregunta;
    public TMP_Text textoPregunta;               // TextMeshProUGUI
    public Button[] botonesOpciones;             // botones que muestran las opciones

    // Estado actual
    private Pregunta preguntaActual;
    private TerminalSeguridad terminalActual;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✅ UIManager inicializado correctamente en " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager duplicado destruido.");
            Destroy(gameObject);
            return;
        }

        if (panelPregunta != null)
            panelPregunta.SetActive(false);
    }

    // Mostrar un panel con la pregunta que pasa un terminal
    public void MostrarPanelPregunta(Pregunta p, TerminalSeguridad terminal)
    {
        if (p == null)
        {
            Debug.LogError("UIManager: la pregunta es null.");
            return;
        }

        preguntaActual = p;
        terminalActual = terminal;

        if (panelPregunta == null || textoPregunta == null || botonesOpciones == null)
        {
            Debug.LogError("UIManager: referencias UI no asignadas en el Inspector.");
            return;
        }

        // Mostrar texto y botones
        panelPregunta.SetActive(true);
        textoPregunta.text = preguntaActual.textoPregunta;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < preguntaActual.opciones.Length)
            {
                botonesOpciones[i].gameObject.SetActive(true);
                // si el child del botón tiene TMP text, lo asignamos:
                TMP_Text t = botonesOpciones[i].GetComponentInChildren<TMP_Text>();
                if (t != null) t.text = preguntaActual.opciones[i];
                else
                {
                    // fallback: si usas legacy Text
                    Text tLegacy = botonesOpciones[i].GetComponentInChildren<Text>();
                    if (tLegacy != null) tLegacy.text = preguntaActual.opciones[i];
                }

                // listeners
                int index = i;
                botonesOpciones[i].onClick.RemoveAllListeners();
                botonesOpciones[i].onClick.AddListener(() => BotonOpcionPresionado(index));
            }
            else
            {
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }

        Debug.Log("🧠 UIManager: mostrando pregunta -> " + preguntaActual.textoPregunta);
    }

    void BotonOpcionPresionado(int indice)
    {
        bool correcta = (preguntaActual != null) && (indice == preguntaActual.respuestaCorrecta);

        // Cerrar panel
        panelPregunta.SetActive(false);

        // Notificar al terminal (si existe)
        if (terminalActual != null)
            terminalActual.ResolverPregunta(correcta);
        else
            Debug.LogWarning("UIManager: terminalActual es null al validar respuesta.");

        // Limpiar estado
        preguntaActual = null;
        terminalActual = null;
    }
}
