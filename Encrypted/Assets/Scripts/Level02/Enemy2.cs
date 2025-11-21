using UnityEngine;

public class Enemy2 : Enemy
{
    [Header("Enemy2 Detection Range")]
    [SerializeField] private float detectionRange = 5f;
    
    [Header("Contact Damage")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 1f;
    
    private bool isPlayerInRange = false;
    private float lastDamageTime = -999f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isPlayerInRange = distanceToPlayer <= detectionRange;
            
            playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
        }
        else
        {
            isPlayerInRange = false;
            playerDetected = false;
        }
    }

    protected override void HandleMovement()
    {
        if (!isPlayerInRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (!canMove || player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (!playerDetected)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
            {
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                DamagePlayer(collision.gameObject);
            }
        }
    }

    private void DamagePlayer(GameObject playerObject)
    {
        Entity playerEntity = playerObject.GetComponent<Entity>();
        if (playerEntity != null)
        {
            playerEntity.BalaDamage(contactDamage);
            lastDamageTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}





