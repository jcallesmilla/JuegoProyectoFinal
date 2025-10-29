using System;
using UnityEngine;


public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    public CoinManager cm;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8;
    protected int facingDir = 1;
    private float xInput;
    private bool facingRight = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    protected bool canMove = true;
    private bool canJump = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity enemyTarget = enemy.GetComponent<Entity>();
            enemyTarget.TakeDamage();
        }
    }

    public void TakeDamage()
    {
        throw new NotImplementedException();
    }


    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }
    protected void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryToAttack();
        }
    }

    protected virtual void TryToAttack()
    {
        if (isGrounded){
            anim.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
            
    }
    
    protected virtual void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
            
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            Flip();
        }else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject); 
            cm.coinCount++;
        }
    }

}
