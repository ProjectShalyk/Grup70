using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float damageAmount = 10f;
    public float knockbackForce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.GetDamage(damageAmount);

            Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
