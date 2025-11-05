using UnityEngine;

public class TerminalSeguridad : MonoBehaviour
{
    public ListaPreguntas listaPreguntas; // puedes asignar en Start o desde inspector si quieres

    private void Start()
    {
        Debug.Log("✅ TerminalSeguridad inicializada en: " + gameObject.name);

        // Si no le asignaste preguntas desde editor, crea una lista básica aquí
        if (listaPreguntas == null)
        {
            listaPreguntas = new ListaPreguntas();
            listaPreguntas.Agregar(new Pregunta("¿Cuál de estas contraseñas es más segura?", new string[] { "12345", "qwerty", "C@0sM1c#21" }, 2));
            listaPreguntas.Agregar(new Pregunta("¿Qué debes evitar al crear una contraseña?", new string[] { "Usar tu nombre", "Combinar letras y símbolos", "Hacerla larga" }, 0));
            listaPreguntas.Agregar(new Pregunta("¿Qué mejora la seguridad de una cuenta?", new string[] { "2FA", "Reutilizar contraseñas", "Compartir claves" }, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔵 OnTriggerEnter2D detectado con: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("🟢 Jugador detectado. Mostrando pregunta...");
            Pregunta p = listaPreguntas.ObtenerPreguntaAleatoria();

            if (UIManager.Instance == null)
            {
                Debug.LogError("❌ UIManager.Instance es NULL. Asegúrate de que CanvasPreguntas con UIManager esté en la escena y activo.");
                return;
            }

            UIManager.Instance.MostrarPanelPregunta(p, this);
            Time.timeScale = 0f; // pausa
        }
        else
        {
            Debug.Log("⚪ Trigger detectó objeto que no es Player: " + other.tag);
        }
    }

    // Llamado por UIManager cuando el jugador responde
    public void ResolverPregunta(bool acierto)
    {
        Time.timeScale = 1f; // reanuda el juego
        if (acierto)
        {
            Debug.Log("✅ Respuesta correcta. Terminal desbloqueada.");
            // Aquí puedes activar checkpoint, abrir puerta, guardar progreso, etc.
        }
        else
        {
            Debug.Log("❌ Respuesta incorrecta. Spawnear virus...");
            // Lógica para spawnear enemigos: llama a tu EnemySpawner o instancia prefabs
        }
    }

    // Aux: dibujar el collider en escena si quieres verificar visualmente
    void OnDrawGizmosSelected()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
        }
    }
}
