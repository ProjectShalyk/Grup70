using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.centerOfMass = new Vector2(transform.localScale.x / 2, 0f); 
        rb.freezeRotation = false;
    }

    void Update()
    {
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            return;
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (rb.bodyType != RigidbodyType2D.Dynamic)
    //        return;

    //    rb.linearVelocity = Vector2.zero;
    //    rb.angularVelocity = 0f;
    //    rb.bodyType = RigidbodyType2D.Static;

    //    transform.SetParent(collision.transform.parent.parent.parent, true);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            return;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;


        if (collision.CompareTag("Enemy"))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(collision.transform, true);

            Enemy enemyComp = collision.GetComponent<Enemy>();
            enemyComp.GetDamage(damage);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.SetParent(collision.transform.GetComponentInParent<UniverseController>().transform, true);
        }

    }

}
