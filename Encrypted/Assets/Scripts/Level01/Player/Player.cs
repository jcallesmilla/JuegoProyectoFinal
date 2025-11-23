using UnityEngine;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] private HealthBarPlayer healthBarPlayer;

    private float xInput;
    private bool canJump = true;
    private PlayerStats playerStats;
    private float moveSpeed;
    private float jumpForce;

    protected override void Awake()
    {
        base.Awake();
        
        playerStats = GameManager.Instance.playerStats;
        
        UpdateStatsFromPlayerStats();
        
        currentHealth = maxHealth;
        healthBarPlayer.setMaxHealth(maxHealth);
    }

    protected override void Update()
    {
        base.Update();
        UpdateStatsFromPlayerStats();
        HandleInput();
    }

    private void UpdateStatsFromPlayerStats()
    {
        if (playerStats != null)
        {
            moveSpeed = playerStats.GetCurrentSpeed();
            jumpForce = playerStats.GetCurrentJumpForce();
        }
        else
        {
            moveSpeed = 3.5f;
            jumpForce = 18f;
        }
    }

    protected override void HandleMovement()
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


    private void HandleInput()
{
    xInput = Input.GetAxisRaw("Horizontal");
    HandleMovement();

    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    {
        TryToJump();
    }

    // Swapped: Left click (Mouse0) now calls HandleShoot()
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
        HandleShoot();
    }

    // Swapped: Right click (Mouse1) now calls HandleAttack()
    if (Input.GetKeyDown(KeyCode.Mouse1))
    {
        HandleAttack();
    }
}


    private void TryToJump()
    {
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
    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    protected override void TakeDamage()
    {
        base.TakeDamage();
        healthBarPlayer.SetHealth(currentHealth);
    }

    public override void BalaDamage(int damageAmount)
{
    base.BalaDamage(damageAmount);
    healthBarPlayer.SetHealth(currentHealth);
}

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }

    private void HandleShoot()
{
    if (anim != null)
    {
        anim.SetTrigger("shoot");
    }
    
    PlayerShoot shootScript = GetComponent<PlayerShoot>();
    if (shootScript != null)
    {
        shootScript.Shoot();
    }
}

}