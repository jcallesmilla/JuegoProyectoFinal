using UnityEngine;

public class JEFE2 : Enemy
{
    public enum BossState
    {
        Idle,
        Walking,
        Attack,
        Attack2
    }

    [Header("JEFE2 Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackStateDuration = 2f;
    [SerializeField] private float aboveThreshold = 2f;
    [SerializeField] private LayerMask helicopteroLayer;
    
    [Header("References")]
    [SerializeField] private DisparoJEFE2 disparoController;
    [SerializeField] private HealthBarPlayer jefeHealthBar;

    private BossState currentState = BossState.Idle;
    private float attackTimer;
    private float distanceToHelicoptero;
    private bool canDetectHelicoptero;
    private Transform helicoptero;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
        
        if (disparoController == null)
        {
            disparoController = GetComponent<DisparoJEFE2>();
        }
        
        GameObject helicopteroObject = GameObject.Find("Helicoptero");
        if (helicopteroObject != null)
        {
            helicoptero = helicopteroObject.transform;
            player = helicoptero;
            
            Collider2D helicopteroCollider = helicopteroObject.GetComponent<Collider2D>();
            Collider2D bossCollider = GetComponent<Collider2D>();
            
            if (helicopteroCollider != null && bossCollider != null)
            {
                Physics2D.IgnoreCollision(bossCollider, helicopteroCollider);
            }
        }
        else
        {
            Debug.LogError("JEFE2: Helicoptero not found in scene!");
        }
        
        if (jefeHealthBar != null)
        {
            jefeHealthBar.setMaxHealth(maxHealth);
        }
    }

    protected override void Update()
    {
        if (helicoptero == null) return;

        UpdateHelicopteroDetection();
        UpdateState();
        HandleAnimations();
        HandleCollision();
    }

    private void UpdateHelicopteroDetection()
    {
        distanceToHelicoptero = Vector2.Distance(transform.position, helicoptero.position);
        canDetectHelicoptero = distanceToHelicoptero <= detectionRange;
        
        Vector2 directionToHelicoptero = (helicoptero.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToHelicoptero, detectionRange, helicopteroLayer);
        
        if (hit.collider != null && hit.collider.gameObject.name == "Helicoptero")
        {
            canDetectHelicoptero = true;
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;
            case BossState.Walking:
                HandleWalkingState();
                break;
            case BossState.Attack:
                HandleAttackState();
                break;
            case BossState.Attack2:
                HandleAttack2State();
                break;
        }
    }

    private void HandleIdleState()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (canDetectHelicoptero)
        {
            ChangeState(BossState.Walking);
        }
    }

    private void HandleWalkingState()
    {
        if (!canDetectHelicoptero)
        {
            ChangeState(BossState.Idle);
            return;
        }

        if (distanceToHelicoptero <= attackRange)
        {
            if (IsHelicopteroAbove())
            {
                ChangeState(BossState.Attack);
            }
            else if (IsHelicopteroInFront())
            {
                ChangeState(BossState.Attack2);
            }
            return;
        }

        HandleMovement();
    }

    private void HandleAttackState()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (disparoController != null)
        {
            disparoController.Disparar();
        }
        
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            if (canDetectHelicoptero && distanceToHelicoptero <= attackRange)
            {
                if (IsHelicopteroAbove())
                {
                    attackTimer = attackStateDuration;
                }
                else if (IsHelicopteroInFront())
                {
                    ChangeState(BossState.Attack2);
                }
                else
                {
                    ChangeState(BossState.Walking);
                }
            }
            else if (canDetectHelicoptero)
            {
                ChangeState(BossState.Walking);
            }
            else
            {
                ChangeState(BossState.Idle);
            }
        }
    }

    private void HandleAttack2State()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (disparoController != null)
        {
            disparoController.Disparar();
        }
        
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            if (canDetectHelicoptero && distanceToHelicoptero <= attackRange)
            {
                if (IsHelicopteroInFront())
                {
                    attackTimer = attackStateDuration;
                }
                else if (IsHelicopteroAbove())
                {
                    ChangeState(BossState.Attack);
                }
                else
                {
                    ChangeState(BossState.Walking);
                }
            }
            else if (canDetectHelicoptero)
            {
                ChangeState(BossState.Walking);
            }
            else
            {
                ChangeState(BossState.Idle);
            }
        }
    }

    private bool IsHelicopteroAbove()
    {
        float verticalDistance = helicoptero.position.y - transform.position.y;
        float horizontalDistance = Mathf.Abs(helicoptero.position.x - transform.position.x);
        
        return verticalDistance > aboveThreshold && horizontalDistance < attackRange;
    }

    private bool IsHelicopteroInFront()
    {
        float verticalDistance = Mathf.Abs(helicoptero.position.y - transform.position.y);
        float horizontalDistance = Mathf.Abs(helicoptero.position.x - transform.position.x);
        
        return verticalDistance <= aboveThreshold && horizontalDistance <= attackRange;
    }

    private void ChangeState(BossState newState)
    {
        currentState = newState;

        if (anim == null) return;

        switch (newState)
        {
            case BossState.Idle:
                anim.SetBool("walk", false);
                anim.SetBool("attack", false);
                anim.SetBool("attack2", false);
                anim.SetBool("idle", true);
                break;
            case BossState.Walking:
                anim.SetBool("idle", false);
                anim.SetBool("attack", false);
                anim.SetBool("attack2", false);
                anim.SetBool("walk", true);
                break;
            case BossState.Attack:
                anim.SetBool("idle", false);
                anim.SetBool("walk", false);
                anim.SetBool("attack2", false);
                anim.SetBool("attack", true);
                attackTimer = attackStateDuration;
                break;
            case BossState.Attack2:
                anim.SetBool("idle", false);
                anim.SetBool("walk", false);
                anim.SetBool("attack", false);
                anim.SetBool("attack2", true);
                attackTimer = attackStateDuration;
                break;
        }
    }

    protected override void HandleMovement()
    {
        if (!canMove || helicoptero == null || currentState != BossState.Walking) return;

        float direction = Mathf.Sign(helicoptero.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    protected override void HandleAttack()
    {
    }

    protected override void Flip()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.cyan;
        if (helicoptero != null)
        {
            Gizmos.DrawLine(transform.position, helicoptero.position);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (anim != null)
        {
            anim.SetTrigger("hurt");
        }
        
        if (jefeHealthBar != null)
        {
            jefeHealthBar.SetHealth(currentHealth);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("death");
        }
        
        base.Die();
    }
}



