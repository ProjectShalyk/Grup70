using UnityEngine;
using UnityEngine.UIElements;

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
        {
            animator.SetTrigger("landed");
            AudioManager.Instance.Stop("Jump");
            coyoteCounter = coyoteTime;
        }
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
        float targetSpeed = moveInput * maxSpeed;

        if (targetSpeed != 0 && isGrounded)
        {
            AudioManager.Instance.Play("Run");
            animator.SetBool("running", true);
        }
        else
        {
            AudioManager.Instance.Stop("Run");
            animator.SetBool("running", false);
        }

        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
        rb.AddForce(Vector2.right * movement);


        if (rb.linearVelocity.y > 2f)
            rb.gravityScale = gravityScale;
        else if (rb.linearVelocity.y < -2f)
            rb.gravityScale = gravityScale;
        else
            rb.gravityScale = apexGravityScale;

        if (jumpPressed)
        {
            AudioManager.Instance.Play("Jump");
            animator.SetTrigger("jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }

        FlipIfNeeded();
    }


    void FlipIfNeeded()
    {
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = !transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
    }

    //void Flip()
    //{
    //    facingRight = !facingRight;
    //    Vector3 scale = transform.localScale;
    //    scale.x *= -1;
    //    transform.localScale = scale;
    //}

}
