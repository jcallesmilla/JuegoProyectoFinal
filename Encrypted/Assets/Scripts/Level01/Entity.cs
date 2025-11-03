using System;
using UnityEngine;


public class Entity : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected Rigidbody2D rb;
    public CoinManager cm;

    [Header("Player / Movement")]
    [Tooltip("Si se activa, el flip del sprite se hará invirtiendo localScale.x en vez de rotar el transform")]
    [SerializeField] protected bool useScaleFlip = false;

    [Header("Jump settings")]
    [Tooltip("Número máximo de saltos (1 = salto simple, 2 = doble salto)")]
    [SerializeField] protected int maxJumps = 2;
    // runtime jumps counter
    protected int jumpsLeft;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float jumpForce = 8;
    protected int facingDir = 1;
    protected float xInput;
    protected bool facingRight = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    protected bool canMove = true;
    protected bool canJump = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // If animator not set in inspector, try to find in children
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        // initialize jumpsLeft from serialized value
        jumpsLeft = maxJumps;
        if (anim == null)
            Debug.LogWarning($"Entity ({gameObject.name}): Animator not found. Assign an Animator in the Inspector or add one as a child.");
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
        //throw new NotImplementedException();
    }


    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }
    protected void HandleAnimations()
    {
        if (anim == null) return;
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
            HandleAttack();
        }
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded){
            anim.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        // reset jumps when grounded
        if (isGrounded)
        {
            jumpsLeft = maxJumps;
        }

        if (canJump && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
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
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (useScaleFlip)
        {
            Vector3 s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;
        }
        else
        {
            transform.Rotate(0, 180, 0);
        }
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin") && gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }


}
