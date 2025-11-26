using UnityEngine;

public class BalaJEFE2 : Entity
{
    [Header("Bullet Properties")]
    public Vector2 direccion;
    public float velocidad = 5f;
    public float lifetime = 5f;
    public bool useRigidbody = true;
    public int daño = 1;

    private float timer;
    private bool isInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        timer = lifetime;
    }

    public void Initialize(Vector2 shootDirection, float speed, float life, int damage)
    {
        direccion = shootDirection.normalized;
        velocidad = speed;
        lifetime = life;
        daño = damage;
        timer = lifetime;
        isInitialized = true;
        
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        Destroy(gameObject, lifetime);
    }

    protected override void Update()
    {
        if (!isInitialized && direccion != Vector2.zero)
        {
            float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            isInitialized = true;
        }

        if (useRigidbody && rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
        }
        else
        {
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Helicoptero")
        {
            Entity helicopteroEntity = collision.GetComponent<Entity>();
            if (helicopteroEntity != null)
            {
                helicopteroEntity.BalaDamage(daño);
            }
            
            Destroy(gameObject);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Helicoptero")
        {
            Entity helicopteroEntity = collision.gameObject.GetComponent<Entity>();
            if (helicopteroEntity != null)
            {
                helicopteroEntity.BalaDamage(daño);
            }
            
            Destroy(gameObject);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}


