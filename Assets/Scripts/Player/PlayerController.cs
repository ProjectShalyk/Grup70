using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public partial class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 70f;
    bool facingRight = true;
    Transform hand;


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

    [Header("Combat")]
    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBarImage;
    public bool isDead = false;
    SpriteRenderer spriteRenderer;
    float dissolveAmount = 1f;
    public bool isRespawning = false;
    float respawnDissolveAmount = -0.2f;

    IInteractable currentInteractable;
    CheckpointManager checkpointManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        checkpointManager = CheckpointManager.Instance;
        CombatStart();
        hand = GetComponentInChildren<WeaponController>().hand;
    }

    private void Update()
    {

        InteractUpdate();
        MovementUpdate();
        CombatUpdate();

    }

    private void FixedUpdate()
    {
        MovementFixedUpdate();
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
