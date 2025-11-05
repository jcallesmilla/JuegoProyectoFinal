using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject panelPregunta;
    public Text textoPregunta;
    public Button[] botonesOpciones;

    private Pregunta preguntaActual;
    private TerminalSeguridad terminalActual;

    void Awake()
    {
        instance = this;
    }

    public void MostrarPanelPregunta(Pregunta p, TerminalSeguridad terminal)
    {
        preguntaActual = p;
        terminalActual = terminal;
        panelPregunta.SetActive(true);
        textoPregunta.text = p.texto;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            botonesOpciones[i].GetComponentInChildren<Text>().text = p.opciones[i];
            int index = i;
            botonesOpciones[i].onClick.RemoveAllListeners();
            botonesOpciones[i].onClick.AddListener(() => SeleccionarOpcion(index));
        }
    }

    void SeleccionarOpcion(int indice)
    {
        bool acierto = indice == preguntaActual.indiceCorrecta;
        panelPregunta.SetActive(false);
        terminalActual.ResolverPregunta(acierto);
    }
}
