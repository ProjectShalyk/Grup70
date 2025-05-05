using UnityEngine;

public partial class PlayerController
{
    void CombatStart()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void CombatUpdate()
    {
        if (isDead)
        {
            UpdateDissolveEffect();
            return;
        }
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
}