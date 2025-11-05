using UnityEngine;

public class TerminalSeguridad : MonoBehaviour
{
    public ListaPreguntas listaPreguntas;
    private bool jugadorEnZona;

    void Start()
    {
        listaPreguntas = new ListaPreguntas();

        listaPreguntas.Agregar(new Pregunta(
            "¿Cuál de estas contraseñas es más segura?",
            new string[] { "12345", "qwerty", "C@0sM1c#21" },
            2
        ));
        listaPreguntas.Agregar(new Pregunta(
            "¿Qué debes evitar al crear una contraseña?",
            new string[] { "Usar tu nombre", "Combinar letras y símbolos", "Hacerla larga" },
            0
        ));
        listaPreguntas.Agregar(new Pregunta(
            "¿Qué práctica mejora la seguridad de tus cuentas?",
            new string[] { "Usar 2FA", "Reutilizar contraseñas", "Compartir tus claves" },
            0
        ));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            MostrarPregunta();
        }
    }

    void MostrarPregunta()
    {
        Pregunta p = listaPreguntas.ObtenerAleatoria();
        UIManager.instance.MostrarPanelPregunta(p, this);
        Time.timeScale = 0; // pausa el juego
    }

    public void ResolverPregunta(bool acierto)
    {
        Time.timeScale = 1;
        if (acierto)
        {
            Debug.Log("✅ Terminal desbloqueada!");
            // Aquí puedes activar checkpoint o sonido
        }
        else
        {
            Debug.Log("❌ Error, aparecen virus!");
            // Aquí luego añadiremos los enemigos
        }
    }
}
