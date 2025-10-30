using UnityEngine;

public class BalaJEFE1 : MonoBehaviour
{
    [Header("Bala settings")]
    public float velocidad = 5f;
    public int daño = 10;
    [Tooltip("Tiempo en segundos tras el cual la bala se destruye automáticamente si no choca")]
    public float lifetime = 5f;
    [Tooltip("Dirección en la que se mueve la bala (por defecto izquierda)")]
    public Vector2 direccion = Vector2.left;
    [Tooltip("Si true, la bala usará Rigidbody2D.linearVelocity; si false, se moverá con transform.Translate")]
    public bool useRigidbody = true;

    Rigidbody2D rb;

    void Start()
    {
        // Destruye la bala después de 'lifetime' segundos para evitar que queden balas en escena
        if (lifetime > 0f) Destroy(gameObject, lifetime);

        // Orientar la bala hacia la dirección inicial (opcional, para que el sprite apunte al movimiento)
        if (direccion != Vector2.zero)
        {
            float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (useRigidbody)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direccion.normalized * velocidad;
            }
            else
            {
                Debug.LogWarning("BalaJEFE1: useRigidbody está activado pero no hay Rigidbody2D en el prefab. Usando movimiento por transform como fallback.");
                // Fallback: no Rigidbody available, use transform movement instead
                useRigidbody = false;
                rb = null;
            }
        }
    }

    void Update()
    {
        if (!useRigidbody)
        {
            // Movimiento por transform (no recomendado para física, pero válido para movimientos simples)
            transform.Translate(direccion.normalized * velocidad * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Player"))
        {
            // Intentar aplicar daño usando el componente VidaPlayer si existe
            var vida = collision.GetComponent<VidaPlayer>();
            if (vida != null)
            {
                vida.QuitarVida(daño);
            }
            else
            {
                // Fallback: enviar mensaje (no recomendable pero útil si no hay componente)
                collision.SendMessage("QuitarVida", daño, SendMessageOptions.DontRequireReceiver);
            }

            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
