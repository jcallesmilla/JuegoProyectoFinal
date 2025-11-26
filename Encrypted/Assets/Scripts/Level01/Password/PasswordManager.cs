using UnityEngine;
using TMPro;

public class PasswordManager : MonoBehaviour
{
    public static PasswordManager Instance { get; private set; }

    [Header("Password Configuration")]
    [SerializeField] private bool usarContraseñaFija = false;
    [SerializeField] private string contraseñaFija = "123456";
    [SerializeField] private int longitudContraseña = 6;

    [SerializeField] private int digitosPorFragmento = 2;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI textoFragmentosRevelados;
    [SerializeField] private GameObject panelFragmentoRevelado;
    [SerializeField] private TextMeshProUGUI textoNuevoFragmento;

    private string contraseñaCompleta;
    private string[] fragmentos;
    private bool[] fragmentosRevelados;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GenerarContraseñaAleatoria();
    }

    private void GenerarContraseñaAleatoria()
    {
        if (usarContraseñaFija)
        {
            contraseñaCompleta = contraseñaFija;
        }
        else
        {
            contraseñaCompleta = "";
            for (int i = 0; i < longitudContraseña; i++)
            {
                contraseñaCompleta += Random.Range(0, 10).ToString();
            }
        }

        int numFragmentos = Mathf.CeilToInt((float)longitudContraseña / digitosPorFragmento);
        fragmentos = new string[numFragmentos];
        fragmentosRevelados = new bool[numFragmentos];

        for (int i = 0; i < numFragmentos; i++)
        {
            int inicio = i * digitosPorFragmento;
            int longitud = Mathf.Min(digitosPorFragmento, longitudContraseña - inicio);
            fragmentos[i] = contraseñaCompleta.Substring(inicio, longitud);
            fragmentosRevelados[i] = false;
        }

        Debug.Log($"Contraseña generada: {contraseñaCompleta}");
        ActualizarTextoFragmentos();

        if (panelFragmentoRevelado != null)
            panelFragmentoRevelado.SetActive(false);
    }

    public void RevelarFragmento(int indice)
    {
        if (indice < 0 || indice >= fragmentos.Length)
        {
            Debug.LogWarning($"Índice de fragmento inválido: {indice}");
            return;
        }

        if (fragmentosRevelados[indice])
        {
            Debug.Log($"Fragmento {indice} ya estaba revelado");
            return;
        }

        fragmentosRevelados[indice] = true;
        Debug.Log($"Fragmento {indice} revelado: {fragmentos[indice]}");

        MostrarNotificacionFragmento(fragmentos[indice]);
        ActualizarTextoFragmentos();
    }

    private void MostrarNotificacionFragmento(string fragmento)
    {
        if (panelFragmentoRevelado != null && textoNuevoFragmento != null)
        {
            textoNuevoFragmento.text = $"Fragmento Revelado: {fragmento}";
            panelFragmentoRevelado.SetActive(true);
            Invoke(nameof(OcultarNotificacion), 3f);
        }
    }

    private void OcultarNotificacion()
    {
        if (panelFragmentoRevelado != null)
            panelFragmentoRevelado.SetActive(false);
    }

    private void ActualizarTextoFragmentos()
    {
        if (textoFragmentosRevelados == null) return;

        string pistaVisual = "";
        for (int i = 0; i < fragmentos.Length; i++)
        {
            if (fragmentosRevelados[i])
            {
                pistaVisual += fragmentos[i];
            }
            else
            {
                for (int j = 0; j < fragmentos[i].Length; j++)
                {
                    pistaVisual += "?";
                }
            }
        }

        textoFragmentosRevelados.text = $"Código: {pistaVisual}";
    }

    public string ObtenerContraseñaCompleta()
    {
        return contraseñaCompleta;
    }

    public string ObtenerPistaActual()
    {
        string pista = "";
        for (int i = 0; i < fragmentos.Length; i++)
        {
            if (fragmentosRevelados[i])
            {
                pista += fragmentos[i];
            }
            else
            {
                for (int j = 0; j < fragmentos[i].Length; j++)
                {
                    pista += "?";
                }
            }
        }
        return pista;
    }
}
