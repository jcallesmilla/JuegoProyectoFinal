using UnityEngine;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float jumpForce = 8;
    private float xInput;
    private bool canJump = true;

    protected override void Update()
    {
        base.Update();
        HandleInput();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleAttack();
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
    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }
}
