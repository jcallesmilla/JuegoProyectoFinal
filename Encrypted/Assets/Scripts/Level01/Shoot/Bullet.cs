using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración de Bala")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private float speed = 10f;
    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int dir, float bulletSpeed, int damageAmount)
{
    damage = damageAmount;
    speed = bulletSpeed;

    if (rb != null)
    {
        rb.linearVelocity = new Vector2(dir * speed, 0f);
    }

    if (dir < 0)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }
}


    public void Initialize(Vector2 direction, float bulletSpeed, int damageAmount)
{
    damage = damageAmount;
    speed = bulletSpeed;

    if (rb != null)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0f, 0f, angle);
}


    private void OnTriggerEnter2D(Collider2D collision)
{
    if (hasHit) return;

    if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
    {
        Entity enemyEntity = collision.GetComponent<Entity>();
        if (enemyEntity != null)
        {
            enemyEntity.BalaDamage(damage);
        }

        hasHit = true;
        Destroy(gameObject);
    }
}

}
