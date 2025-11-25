using UnityEngine;
using UnityEngine.InputSystem;

public class HelicopterController : Entity
{
    [Header("Helicopter Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    
    [Header("Helicopter Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float shootCooldown = 0.2f;
    
    [Header("Helicopter Health")]
    [SerializeField] private HealthBarPlayer healthBarPlayer;
    
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private bool canShoot = true;
    private Camera mainCamera;
    
    protected override void Awake()
    {
        base.Awake();
        
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
        
        mainCamera = Camera.main;
        
        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(1f, 0f, 0f);
            firePoint = fp.transform;
        }
        
        currentHealth = maxHealth;
        if (healthBarPlayer != null)
        {
            healthBarPlayer.setMaxHealth(maxHealth);
        }
    }
    
    protected override void Update()
    {
        HandleInput();
        HandleMovement();
        HandleAnimations();
    }
    
    private void HandleInput()
    {
        float horizontal = 0f;
        float vertical = 0f;
        
        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }
        
        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1f;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1f;
        }
        
        moveInput = new Vector2(horizontal, vertical).normalized;
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryShoot();
        }
    }
    
    protected override void HandleMovement()
    {
        if (!canMove)
        {
            currentVelocity = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector2 targetVelocity = moveInput * moveSpeed;
        
        if (moveInput.magnitude > 0)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }
        
        rb.linearVelocity = currentVelocity;
    }
    
    protected override void HandleCollision()
    {
        isGrounded = false;
    }
    
    protected override void TakeDamage()
    {
        base.TakeDamage();
        if (healthBarPlayer != null)
        {
            healthBarPlayer.SetHealth(currentHealth);
        }
    }
    
    public override void BalaDamage(int damageAmount)
    {
        base.BalaDamage(damageAmount);
        if (healthBarPlayer != null)
        {
            healthBarPlayer.SetHealth(currentHealth);
        }
    }
    
    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
    
    private void TryShoot()
    {
        if (!canShoot) return;
        
        if (anim != null)
        {
            anim.SetTrigger("shoot");
        }
        
        SpawnBullet();
        
        Invoke(nameof(ResetShoot), shootCooldown);
        canShoot = false;
    }
    
    private void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null || mainCamera == null) return;
        
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePosition - (Vector2)firePoint.position).normalized;
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(direction, bulletSpeed);
        }
    }
    
    private void ResetShoot()
    {
        canShoot = true;
    }
}

