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
            if (!isRespawning)
            {
                UpdateDissolveEffect(); // dissolve out
            }
            else
            {
                UpdateRespawnEffect(); // dissolve in
            }

            return;
        }

        // normal update kodlarýn buraya gelir
    }

    public void StartRespawn()
    {
        isRespawning = true;
        dissolveAmount = -0.2f;
        transform.position = checkpointManager.GetLastCheckpoint().transform.position;

        spriteRenderer.material.SetFloat("_dissolveAmount", dissolveAmount);

        // Oyuncuyu tekrar aktif hale getir
        GetComponent<Collider2D>().enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
    }
    void UpdateRespawnEffect()
    {
        if (spriteRenderer != null && spriteRenderer.material.HasProperty("_dissolveAmount"))
        {
            dissolveAmount += Time.deltaTime * 1.5f;
            spriteRenderer.material.SetFloat("_dissolveAmount", dissolveAmount);

            if (dissolveAmount >= 1f)
            {
                FinishRespawn();
            }
        }
    }

    void FinishRespawn()
    {
        isDead = false;
        isRespawning = false;
        currentHealth = maxHealth;
        UpdateHealthBar();
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

            if (dissolveAmount < -0.2f)
            {
                spriteRenderer.material.SetFloat("_dissolveAmount", -0.2f);
                checkpointManager.SetPLayerFalse();
            }
        }
    }
}