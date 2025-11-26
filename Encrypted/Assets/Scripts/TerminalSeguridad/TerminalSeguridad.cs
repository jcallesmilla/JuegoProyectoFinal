using UnityEngine;

public class TerminalSeguridad : MonoBehaviour
{
    public GameObject candado;
    public ScreenFlash screenFlash;
    public ListaPreguntas listaPreguntas;

    private bool terminalResuelta = false;
    private bool preguntaEnCurso = false;

    [Header("Password Fragment")]
    public int indiceFragmento = 0;


    private void Start()
    {
        if (listaPreguntas == null)
        {
            listaPreguntas = new ListaPreguntas();

            listaPreguntas.Agregar(new Pregunta(
                "¿Cuál de estas contraseñas es la más segura?",
                new string[] { "12345", "qwerty", "C@m21#" }, 2));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué deberías evitar usar al crear una contraseña?",
                new string[] { "Tu nombre", "símbolos", "frase larga" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué opción proporciona mayor seguridad a tus cuentas?",
                new string[] { "Compartir", "reusar", "2FA" }, 2));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué hace sospechoso a un correo electrónico?",
                new string[] { "Link raro", "logo HD", "redacción formal" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "Si recibes un enlace extraño, ¿qué es lo más seguro hacer?",
                new string[] { "Ignorar", "abrir", "responder" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué no deberías publicar en redes sociales?",
                new string[] { "Clave", "comida", "viaje" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué es el malware?",
                new string[] { "App mala", "editor", "mapa" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "¿Cómo intentan acceder las fuerzas brutas a una cuenta?",
                new string[] { "Probar claves", "hack físico", "apagar wifi" }, 0));

            listaPreguntas.Agregar(new Pregunta(
                "¿Qué actividad es riesgosa en una red wifi pública?",
                new string[] { "Abrir app banco", "hacer compras", "ver videos" }, 0));
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
        if (correcta)
        {
            terminalResuelta = true;
            preguntaEnCurso = false;

            if (candado != null)
                candado.SetActive(false);

            UIManager.Instance.CerrarPanelPregunta();
            Time.timeScale = 1f;

            if (PasswordManager.Instance != null)
            {
                PasswordManager.Instance.RevelarFragmento(indiceFragmento);
            }

            Debug.Log("Terminal desbloqueada.");
        }
        else
        {
            if (screenFlash != null)
                screenFlash.FlashRed();

            UIManager.Instance.CerrarPanelPregunta();

            Debug.Log("Incorrecta. Nueva pregunta.");

            MostrarNuevaPregunta();
        }
    }
}