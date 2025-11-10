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

    [Header("Homing")]
    [Tooltip("Si true, la bala ajustará su dirección para seguir al Player mientras esté viva.")]
    public bool homing = true;
    [Tooltip("Velocidad de giro/interpolación del vector de velocidad cuando es homing (mayor = gira más rápido)")]
    public float homingTurnSpeed = 8f;

    // cached target to follow when homing
    private Transform target;

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

        // Setup homing target if requested
        if (homing)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) target = go.transform;
        }
    }

    void FixedUpdate()
    {
        if (useRigidbody)
        {
            if (homing && target != null && rb != null)
            {
                // desired velocity towards target
                Vector2 toTarget = ((Vector2)target.position - (Vector2)transform.position).normalized * velocidad;
                // smooth velocity change so it 'turns' towards the player
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, toTarget, homingTurnSpeed * Time.fixedDeltaTime);
                // update direccion for fallback visuals
                direccion = rb.linearVelocity.normalized;
                // rotate to face movement direction
                if (direccion != Vector2.zero)
                {
                    float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            else if (rb != null)
            {
                // maintain constant velocity
                rb.linearVelocity = direccion.normalized * velocidad;
            }
        }
        else
        {
            // non-rigidbody fallback movement (still support homing)
            if (homing && target != null)
            {
                Vector2 moveDir = ((Vector2)target.position - (Vector2)transform.position).normalized;
                transform.Translate(moveDir * velocidad * Time.fixedDeltaTime, Space.World);
                direccion = moveDir;
                if (direccion != Vector2.zero)
                {
                    float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            else
            {
                transform.Translate(direccion.normalized * velocidad * Time.fixedDeltaTime, Space.World);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        // If we hit the player via trigger, apply damage
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

        // If hit ground/obstacle via trigger, destroy
        if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    // Also handle non-trigger collisions so the projectile can use a normal collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        if (collision.collider.CompareTag("Player"))
        {
            var vida = collision.collider.GetComponent<VidaPlayer>();
            if (vida != null) vida.QuitarVida(daño);
            else collision.collider.SendMessage("QuitarVida", daño, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
            return;
        }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
