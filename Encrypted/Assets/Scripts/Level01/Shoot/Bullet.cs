using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración de Bala")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private int direction = 1;
    private float speed = 10f;
    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int dir, float bulletSpeed)
    {
        direction = dir;
        speed = bulletSpeed;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }

        if (direction < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            var enemy = collision.GetComponent<MonoBehaviour>();
            if (enemy != null)
            {
                enemy.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }

            hasHit = true;
            Destroy(gameObject);
        }
    }
}