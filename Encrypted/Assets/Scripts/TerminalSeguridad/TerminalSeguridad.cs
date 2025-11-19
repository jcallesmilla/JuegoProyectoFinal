using UnityEngine;

public class TerminalSeguridad : MonoBehaviour
{
    public GameObject candado;
    public ScreenFlash screenFlash;
    public ListaPreguntas listaPreguntas;

    private bool terminalResuelta = false;
    private bool preguntaEnCurso = false;

    private void Start()
    {
        if (listaPreguntas == null)
        {
            listaPreguntas = new ListaPreguntas();

            listaPreguntas.Agregar(new Pregunta(
                "¿Cuál clave es más segura?",
                new string[] { "12345", "qwerty", "C@m21#" }, 2));

            listaPreguntas.Agregar(new Pregunta(
                "Evita usar en claves:",
                new string[] { "TuNombre", "Símbolos", "FraseLarga" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "Mayor seguridad:",
                new string[] { "Compartir", "Reusar", "2FA" }, 2));

            listaPreguntas.Agregar(new Pregunta(
                "Correo sospechoso:",
                new string[] { "LinkRaro", "LogoHD", "Formal" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "Link extraño:",
                new string[] { "Ignorar", "Abrir", "Responder" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "No publiques:",
                new string[] { "Clave", "Comida", "Viaje" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "Malware es:",
                new string[] { "AppMala", "Editor", "Mapa" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "Fuerza bruta:",
                new string[] { "MilesClaves", "HackFísico", "ApagarWiFi" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "WiFi pública:",
                new string[] { "Banco", "Google", "Videos" }, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (terminalResuelta) return;
        if (preguntaEnCurso) return;

        MostrarNuevaPregunta();
    }

    private void MostrarNuevaPregunta()
    {
        preguntaEnCurso = true;

        Pregunta p = listaPreguntas.ObtenerPreguntaAleatoria();
        UIManager.Instance.MostrarPanelPregunta(p, this);

        Time.timeScale = 0f;
    }

    public void ResolverPregunta(bool correcta)
    {
        // RESPUESTA CORRECTA → TERMINA TERMINAL
        if (correcta)
        {
            terminalResuelta = true;
            preguntaEnCurso = false;

            if (candado != null)
                candado.SetActive(false);

            UIManager.Instance.CerrarPanelPregunta();
            Time.timeScale = 1f;

            Debug.Log("Terminal desbloqueada.");
        }
        else
        {
            // RESPUESTA INCORRECTA
            if (screenFlash != null)
                screenFlash.FlashRed();

            UIManager.Instance.CerrarPanelPregunta();

            Debug.Log("Incorrecta. Nueva pregunta.");

            // NUEVA PREGUNTA INMEDIATA (PAUSA ACTIVA)
            MostrarNuevaPregunta();
        }
    }
}