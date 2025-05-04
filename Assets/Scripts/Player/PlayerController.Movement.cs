using UnityEngine;

public partial class PlayerController
{
    void MovementUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            bufferCounter = jumpBufferTime;
        else
            bufferCounter -= Time.deltaTime;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        if (coyoteCounter > 0f && bufferCounter > 0f)
        {
            jumpPressed = true;
            bufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }

    void MovementFixedUpdate()
    {
        // ivmelenme
        float targetSpeed = moveInput * maxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
        rb.AddForce(Vector2.right * movement);

        // gravity
        if (rb.linearVelocity.y > 2f)
            rb.gravityScale = gravityScale;
        else if (rb.linearVelocity.y < -2f)
            rb.gravityScale = gravityScale;
        else
            rb.gravityScale = apexGravityScale;

        // jump
        if (jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }
}