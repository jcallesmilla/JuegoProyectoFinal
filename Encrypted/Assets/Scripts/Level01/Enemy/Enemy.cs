using UnityEngine;

public class Enemy : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    protected bool playerDetected;
    protected Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected)
        {
            anim.SetTrigger("attack");
        }
    }

    protected override void HandleMovement()
    {
        if (!canMove || player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (!playerDetected)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            // Voltearse si no está mirando al jugador
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

    protected override void Flip()
    {
        // Solo voltea visualmente el sprite, sin alterar facingDir ni rotar el transform
        facingRight = !facingRight;

        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    protected override void TakeDamage()
    {
        base.TakeDamage(); // sigue restando vida y reproduce feedback
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
