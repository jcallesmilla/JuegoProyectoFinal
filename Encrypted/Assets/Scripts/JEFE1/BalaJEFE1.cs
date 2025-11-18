using UnityEngine;

public class BalaJEFE1 : Entity
{
    [Header("Bullet Properties")]
    public Vector2 direccion;
    public float velocidad = 5f;
    public float lifetime = 5f;
    public bool useRigidbody = true;
    public int daño = 1;

    private float timeAlive = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        // --- Movement ---
        if (useRigidbody && rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
        }
        else
        {
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        }

        // --- Rotation so bullet faces its direction ---
        if (direccion != Vector2.zero)
        {
            float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Entity playerEntity = collision.GetComponent<Entity>();
            if (playerEntity != null)
            {
                playerEntity.BalaDamage(daño);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

    protected override void HandleMovement() { }
    protected override void HandleCollision() { }
}

