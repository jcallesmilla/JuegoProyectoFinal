using System;
using UnityEngine;
using System.Collections;


public class Entity : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected Rigidbody2D rb;
    //public CoinManager cm; //no se necesita
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] public int maxHealth = 1;
    [SerializeField] public int currentHealth;
    [SerializeField] protected HealthBar_Behaviour healthBar;

    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Player / Movement")]
    [Tooltip("Si se activa, el flip del sprite se hará invirtiendo localScale.x en vez de rotar el transform")]
    [SerializeField] protected bool useScaleFlip = false;

    [Header("Jump settings")]
    [Tooltip("Número máximo de saltos (1 = salto simple, 2 = doble salto)")]
    [SerializeField] protected int maxJumps = 2;
    protected int jumpsLeft;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    protected bool isGrounded;
    protected bool canMove = true;
    protected int facingDir = 1;
    protected bool facingRight = true;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

    }

    protected virtual void Update()
    {
        HandleCollision();
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

    protected virtual void TakeDamage()
    {
        currentHealth = currentHealth - 1;
        PlayDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
        {
            StopCoroutine(damageFeedbackCoroutine);
        }
        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        Material originalMaterial = sr.material;
        sr.material = damageMaterial;
        yield return new WaitForSeconds(damageFeedbackDuration);
        sr.material = originalMaterial;
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 0);
    }


    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }
    protected void HandleAnimations()
    {
        if (anim == null) return;
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
        }
    }

    protected virtual void HandleMovement()
    {
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

    protected virtual void Flip()
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
        facingDir *= -1;

        if (attackPoint != null)
        {
            Vector3 ap = attackPoint.localPosition;
            ap.x *= -1f;
            attackPoint.localPosition = ap;
        }
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
            GameManager.Instance.AddCoins(1);
        }
    }
public virtual void BalaDamage(int damageAmount)
{
    currentHealth = currentHealth - damageAmount;
    PlayDamageFeedback();

    if (currentHealth <= 0)
    {
        Die();
    }
}
}
