using UnityEngine;

public class JEFE1 : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;                 // Referencia al objeto Player
    public Animator animator;                // Controlador de animaciones
    public float velocidad = 2f;             // Velocidad de movimiento
    public float rangoDeteccion = 8f;        // Distancia a la que detecta al jugador

    private bool mirandoDerecha = true;      // Indica hacia qué lado mira el jefe

    void Update()
    {
        if (player == null) return; // Si no hay jugador asignado, salir

        float distancia = Vector2.Distance(transform.position, player.position);

        // El jefe siempre mira hacia el jugador
        MirarHaciaJugador();

        if (distancia <= rangoDeteccion)
        {
            // Si el jugador está dentro del rango, camina hacia él
            animator.SetBool("isWalking", true);
            MoverHaciaJugador();
        }
        else
        {
            // Si el jugador está lejos, se queda en idle
            animator.SetBool("isWalking", false);
        }
    }

    void MoverHaciaJugador()
    {
        // Dirección horizontal hacia el jugador
        Vector2 direccion = (player.position - transform.position).normalized;
        transform.position = new Vector2(transform.position.x + direccion.x * velocidad * Time.deltaTime, transform.position.y);
    }

    void MirarHaciaJugador()
    {
        // Si el jugador está a la derecha y el jefe no mira a la derecha → voltear
        if (player.position.x > transform.position.x && !mirandoDerecha)
        {
            Voltear();
        }
        // Si el jugador está a la izquierda y el jefe mira a la derecha → voltear
        else if (player.position.x < transform.position.x && mirandoDerecha)
        {
            Voltear();
        }
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // Dibuja el rango de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
