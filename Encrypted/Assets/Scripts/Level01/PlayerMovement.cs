using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed;
    private float horizontal;
    private float vertical;
    private float jumpForce;
    
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;
    
    private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        groundLayer = LayerMask.GetMask("Ground");
        
        playerStats = GameManager.Instance.playerStats;
        UpdateStatsFromPlayerStats();
    }

    void Update()
    {
        UpdateStatsFromPlayerStats();
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Direction();
        
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    
    private void UpdateStatsFromPlayerStats()
    {
        if (playerStats != null)
        {
            speed = playerStats.GetCurrentSpeed();
            jumpForce = playerStats.GetCurrentJumpForce();
        }
        else
        {
            speed = 5.0f;
            jumpForce = 7.0f;
        }
    }
    
    private void Direction()
    {
        if (isFacingRight  && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = playerTransform.localScale;
            localscale.x *= -1;
            playerTransform.localScale = localscale;
        }
    }
    
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(playerTransform.position, 0.1f, groundLayer);
    }
}
    