using UnityEngine;

public class Jefe1 : Enemy
{
    public enum BossState
    {
        Idle,
        Running,
        Shooting
    }

    [Header("JEFE1 Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float shootingStateDuration = 2f;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("References")]
    [SerializeField] private DisparoJEFE1 disparoController;

    private BossState currentState = BossState.Idle;
    private float shootingTimer;
    private float distanceToPlayer;
    private bool canDetectPlayer;
    private float shootCooldown;

protected override void Awake()
{
    base.Awake();
    
    if (disparoController == null)
    {
        disparoController = GetComponent<DisparoJEFE1>();
    }
    
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null)
    {
        Collider2D playerCollider = playerObject.GetComponent<Collider2D>();
        Collider2D bossCollider = GetComponent<Collider2D>();
        
        if (playerCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(bossCollider, playerCollider);
        }
    }
}


    protected override void Update()
    {
        if (player == null) return;

        UpdatePlayerDetection();
        UpdateState();
        HandleAnimations();
        HandleCollision();
        HandleFlip();

        if (shootCooldown > 0f) shootCooldown -= Time.deltaTime;
    }

    private void UpdatePlayerDetection()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        canDetectPlayer = distanceToPlayer <= detectionRange;
        
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, playerLayer);
        
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            canDetectPlayer = true;
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;
            case BossState.Running:
                HandleRunningState();
                break;
            case BossState.Shooting:
                HandleShootingState();
                break;
        }
    }

    private void HandleIdleState()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (canDetectPlayer)
        {
            ChangeState(BossState.Running);
        }
    }

    private void HandleRunningState()
    {
        if (!canDetectPlayer)
        {
            ChangeState(BossState.Idle);
            return;
        }

        if (distanceToPlayer <= shootingRange)
        {
            ChangeState(BossState.Shooting);
            return;
        }

        HandleMovement();
    }

    private void HandleShootingState()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (disparoController != null && disparoController.fireRate > 0f)
        {
            if (shootCooldown <= 0f)
            {
                disparoController.Disparar();
                shootCooldown = 1f / disparoController.fireRate;
            }
        }
        
        shootingTimer -= Time.deltaTime;

        if (shootingTimer <= 0f)
        {
            if (canDetectPlayer && distanceToPlayer <= shootingRange)
            {
                shootingTimer = shootingStateDuration;
            }
            else if (canDetectPlayer)
            {
                ChangeState(BossState.Running);
            }
            else
            {
                ChangeState(BossState.Idle);
            }
        }
    }

    private void ChangeState(BossState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case BossState.Idle:
                anim.SetBool("isRunning", false);
                anim.SetBool("isShooting", false);
                break;
            case BossState.Running:
                anim.SetBool("isRunning", true);
                anim.SetBool("isShooting", false);
                break;
            case BossState.Shooting:
                anim.SetBool("isRunning", false);
                anim.SetBool("isShooting", true);
                shootingTimer = shootingStateDuration;
                break;
        }
    }

    protected override void HandleMovement()
    {
        if (!canMove || player == null || currentState != BossState.Running) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    protected override void HandleAttack()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}




