using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public Transform sword;

    public float speed = 2f;
    public float attackRange = 1f;
    public float chaseRange = 5f;
    public float attackCooldown = 1.5f;
    public float maxHealth = 100f;

    public Image healthBarImage;

    private float currentHealth;
    private bool isDead = false;
    private float dissolveAmount = 1f;

    private Vector3 nextPatrolPoint;
    private bool isChasing = false;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        nextPatrolPoint = pointB.position;
        currentHealth = maxHealth;

    }

    void Update()
    {
        if (isDead)
        {
            UpdateDissolveEffect();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool playerInPatrolZone = IsPlayerBetweenPoints();

        if (playerInPatrolZone && distanceToPlayer <= chaseRange)
            isChasing = true;
        else if (distanceToPlayer > chaseRange)
            isChasing = false;

        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        Vector2 direction = (nextPatrolPoint - transform.position);
        direction.y = 0;
        direction = direction.normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, nextPatrolPoint) < 0.2f)
        {
            nextPatrolPoint = (nextPatrolPoint == pointA.position) ? pointB.position : pointA.position;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position);
        direction.y = 0;
        direction = direction.normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
            animator.Play("SwordSwing");
        // damage player (implement elsewhere)
    }

    public void GetDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        animator.enabled = false;
    }

    void UpdateDissolveEffect()
    {
        if (spriteRenderer != null && spriteRenderer.material.HasProperty("_dissolveAmount"))
        {
            dissolveAmount -= Time.deltaTime * 1.5f; // speed of dissolve
            spriteRenderer.material.SetFloat("_dissolveAmount", dissolveAmount);

            if (dissolveAmount <= -0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    bool IsPlayerBetweenPoints()
    {
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        return player.position.x > minX && player.position.x < maxX;
    }

    private void OnDrawGizmosSelected()
    {
        if (sword == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sword.position, attackRange);
    }
}
