using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject panelPregunta;
    public TMP_Text textoPregunta;
    public Button[] botonesOpciones;

    private Pregunta preguntaActual;
    private TerminalSeguridad terminalActual;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("UIManager inicializado en: " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (panelPregunta != null)
            panelPregunta.SetActive(false);
    }

    public void MostrarPanelPregunta(Pregunta p, TerminalSeguridad terminal)
    {
        preguntaActual = p;
        terminalActual = terminal;

        panelPregunta.SetActive(true);
        textoPregunta.text = preguntaActual.textoPregunta;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < preguntaActual.opciones.Length)
            {
                botonesOpciones[i].gameObject.SetActive(true);

                TMP_Text t = botonesOpciones[i].GetComponentInChildren<TMP_Text>();
                if (t != null) t.text = preguntaActual.opciones[i];

                int index = i;
                botonesOpciones[i].onClick.RemoveAllListeners();
                botonesOpciones[i].onClick.AddListener(() => BotonOpcionPresionado(index));
            }
            else
            {
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }
    }

    private void BotonOpcionPresionado(int indice)
    {
        bool correcta = (preguntaActual != null && indice == preguntaActual.respuestaCorrecta);

        var tmpTerminal = terminalActual;

        // Limpiar ANTES de cerrar
        terminalActual = null;
        preguntaActual = null;

        panelPregunta.SetActive(false);

        if (tmpTerminal != null)
            tmpTerminal.ResolverPregunta(correcta);
        else
            Debug.LogWarning("UIManager: terminalActual era null al validar.");
    }

    public void CerrarPanelPregunta()
    {
        panelPregunta.SetActive(false);
    }
}