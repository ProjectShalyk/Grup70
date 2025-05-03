using UnityEngine;

namespace abc
{

    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 8f;
        [SerializeField] private float acceleration = 50f;
        [SerializeField] private float deceleration = 70f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private float gravityScale = 3f;
        [SerializeField] private float apexGravityScale = 1.5f;

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Coyote & Buffer")]
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.15f;

        private Rigidbody2D rb;
        private float moveInput;
        private bool jumpPressed;
        private float coyoteCounter;
        private float bufferCounter;
        private bool isGrounded;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
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

        private void FixedUpdate()
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

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }

    }
}
