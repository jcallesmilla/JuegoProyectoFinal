using UnityEngine;
using System;

public class FlyDrone : Enemy
{
    public static event Action<FlyDrone> OnDroneDestroyed;

    [Header("FlyDrone Settings")]
    public float attackDistance = 3f;
    public float separationDistance = 2f;
    public float separationForce = 2f;
    
    [Header("Combat Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public int bulletDamage = 1;
    
    [Header("Spawn Settings")]
    public float activationDelay = 0.5f;
    
    [Header("Collision Settings")]
    public bool ignoreGroundCollision = true;
    public bool ignoreWallCollision = true;
    
    private float nextFireTime;
    private bool isActive = false;
    public bool isDead = false;
    private float distanceToPlayer;
    private Vector2 lastPosition;

    protected override void Awake()
    {
        base.Awake();

        //SetupCollisionIgnoring();

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            firePoint = fp.transform;
        }

        lastPosition = transform.position;
        
        Invoke(nameof(Activate), activationDelay);
    }

    private void SetupCollisionIgnoring()
{
    Collider2D droneCollider = GetComponent<Collider2D>();
    
    if (droneCollider == null)
    {
        Debug.LogWarning("FlyDrone: No Collider2D found on this GameObject!");
        return;
    }

    if (ignoreGroundCollision || ignoreWallCollision)
    {
        Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        
        foreach (Collider2D col in allColliders)
        {
            if (col == droneCollider) continue;
            
            if (col.CompareTag("Player") || col.GetComponent<Bullet>() != null)
            {
                continue;
            }
            
            bool shouldIgnore = false;
            
            if (ignoreGroundCollision)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                    col.gameObject.layer == LayerMask.NameToLayer("Suelo") ||
                    col.CompareTag("Pared"))
                {
                    shouldIgnore = true;
                }
            }
            
            if (ignoreWallCollision)
            {
                if (col.CompareTag("Pared"))
                {
                    shouldIgnore = true;
                }
            }
            
            if (shouldIgnore)
            {
                Physics2D.IgnoreCollision(droneCollider, col, true);
            }
        }
    }
}


    private void Activate()
    {
        isActive = true;
    }

    protected override void Update()
    {
        if (!isActive || isDead || player == null) return;

        HandleMovement();
        Flip();
        HandleShooting();
        HandleAnimations();
    }

    protected override void HandleMovement()
    {
        if (!canMove || player == null) return;

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            Vector2 separationVector = CalculateSeparation();
            
            Vector2 finalDirection = (directionToPlayer + separationVector * separationForce).normalized;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                (Vector2)transform.position + finalDirection, 
                moveSpeed * Time.deltaTime);
        }
        else
        {
            Vector2 separationVector = CalculateSeparation();
            
            if (separationVector.magnitude > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, 
                    (Vector2)transform.position + separationVector, 
                    separationForce * Time.deltaTime);
            }
            else if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private Vector2 CalculateSeparation()
    {
        FlyDrone[] allDrones = FindObjectsByType<FlyDrone>(FindObjectsSortMode.None);
        Vector2 separationVector = Vector2.zero;
        int nearbyDrones = 0;

        foreach (FlyDrone otherDrone in allDrones)
        {
            if (otherDrone == this || otherDrone.isDead) continue;

            float distance = Vector2.Distance(transform.position, otherDrone.transform.position);

            if (distance < separationDistance && distance > 0)
            {
                Vector2 awayFromOther = (Vector2)(transform.position - otherDrone.transform.position);
                separationVector += awayFromOther.normalized / distance;
                nearbyDrones++;
            }
        }

        if (nearbyDrones > 0)
        {
            separationVector /= nearbyDrones;
        }

        return separationVector;
    }

    private void HandleShooting()
    {
        if (distanceToPlayer <= attackDistance && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (anim != null)
        {
            anim.SetTrigger("fire");
        }

        if (bulletPrefab != null && firePoint != null && player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            FlyDroneBullet bulletScript = bullet.GetComponent<FlyDroneBullet>();
            
            if (bulletScript != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                bulletScript.Initialize(direction, bulletDamage);
            }
        }
    }

    protected new void HandleAnimations()
    {
        if (anim == null || player == null) return;

        Vector2 currentPosition = transform.position;
        Vector2 movementDirection = currentPosition - lastPosition;

        float movementTowardsPlayer = Vector2.Dot(movementDirection.normalized, (player.position - transform.position).normalized);

        if (movementDirection.magnitude > 0.01f)
        {
            if (movementTowardsPlayer > 0)
            {
                anim.SetBool("forward", true);
                anim.SetBool("back", false);
            }
            else
            {
                anim.SetBool("forward", false);
                anim.SetBool("back", true);
            }
        }
        else
        {
            anim.SetBool("forward", false);
            anim.SetBool("back", false);
        }

        lastPosition = currentPosition;
    }

public override void BalaDamage(int damageAmount)
{
    if (isDead) return;

    currentHealth -= damageAmount;
    
    if (healthBar != null)
    {
        healthBar.SetHealth(currentHealth, maxHealth);
    }
    
    PlayDamageFeedback();

    if (currentHealth <= 0)
    {
        Die();
    }
}


    protected override void Die()
    {
        if (isDead) return;
        
        isDead = true;
        isActive = false;

        if (anim != null)
        {
            anim.SetTrigger("death");
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        OnDroneDestroyed?.Invoke(this);

        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        if (isDead)
        {
            OnDroneDestroyed?.Invoke(this);
        }
    }

    protected override void HandleCollision()
    {
    }

    protected override void HandleAttack()
    {
    }

    protected override void Flip()
    {
        if (player == null) return;

        float directionX = player.position.x - transform.position.x;
        
        if (directionX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}






