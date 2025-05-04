using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject WrapperPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.centerOfMass = new Vector2(transform.localScale.x / 2, 0f);  // Pivot noktasý ortadaysa
        rb.freezeRotation = false;
    }

    void Update()
    {
        // Uç kýsmý yere bakmalý: velocity yönüne dön
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            return;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        transform.SetParent(collision.transform.parent.parent.parent, true); // dünya uzayýna göre sabit kal
    }



}
