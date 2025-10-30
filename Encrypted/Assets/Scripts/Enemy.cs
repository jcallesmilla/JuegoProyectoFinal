using UnityEngine;

public class Enemy : Entity
{

    private bool playerDetected;

    protected override void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected)
        {
            anim.SetTrigger("attack");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red; // color de la esfera
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }



    protected override void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }
}
