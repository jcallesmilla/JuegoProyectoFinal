using UnityEngine;

public class FlyDroneBullet : MonoBehaviour
{
    [Header("Movement Settings")]
    public float bulletSpeed = 8f;
    public float lifetime = 5f;
    
    [Header("Collision Settings")]
    public bool ignoreGroundCollision = true;
    public bool ignoreWallCollision = true;
    
    private Vector2 direction;
    private int damage;
    private bool isInitialized = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("FlyDroneBullet: No SpriteRenderer found!");
        }
        else if (spriteRenderer.sprite == null)
        {
            Debug.LogError("FlyDroneBullet: No sprite assigned to SpriteRenderer!");
        }

        SetupCollisionIgnoring();
    }

    private void SetupCollisionIgnoring()
    {
        Collider2D bulletCollider = GetComponent<Collider2D>();
        
        if (bulletCollider == null)
        {
            Debug.LogWarning("FlyDroneBullet: No Collider2D found on this GameObject!");
            return;
        }

        if (ignoreGroundCollision)
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1)
            {
                int bulletLayer = gameObject.layer;
                Physics2D.IgnoreLayerCollision(bulletLayer, groundLayer, true);
            }

            int sueloLayer = LayerMask.NameToLayer("Suelo");
            if (sueloLayer != -1)
            {
                int bulletLayer = gameObject.layer;
                Physics2D.IgnoreLayerCollision(bulletLayer, sueloLayer, true);
            }

            GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Pared");
            foreach (GameObject groundObj in groundObjects)
            {
                Collider2D groundCollider = groundObj.GetComponent<Collider2D>();
                if (groundCollider != null)
                {
                    Physics2D.IgnoreCollision(bulletCollider, groundCollider, true);
                }
            }
        }

        if (ignoreWallCollision)
        {
            GameObject[] wallObjects = GameObject.FindGameObjectsWithTag("Pared");
            foreach (GameObject wallObj in wallObjects)
            {
                Collider2D wallCollider = wallObj.GetComponent<Collider2D>();
                if (wallCollider != null)
                {
                    Physics2D.IgnoreCollision(bulletCollider, wallCollider, true);
                }
            }
        }

        Collider2D[] allGroundColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (Collider2D col in allGroundColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                col.gameObject.layer == LayerMask.NameToLayer("Suelo"))
            {
                Physics2D.IgnoreCollision(bulletCollider, col, true);
            }
        }

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, enemyCollider, true);
            }
        }
    }

    public void Initialize(Vector2 shootDirection, int bulletDamage)
    {
        direction = shootDirection.normalized;
        damage = bulletDamage;
        isInitialized = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!isInitialized) return;

        transform.position += (Vector3)direction * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Entity playerEntity = other.GetComponent<Entity>();
            if (playerEntity != null)
            {
                playerEntity.BalaDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (isInitialized)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * 0.5f);
        }
    }
}

