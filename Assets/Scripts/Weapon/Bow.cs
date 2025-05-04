using UnityEngine;
using UnityEngine.UI;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float maxArrowForce = 30f;
    public float minArrowForce = 5f;
    public float maxChargeTime = 2f;
    public float reloadTime = 1f; // time to reload after shooting

    public Image chargeBarUI; // Unity UI Image (fill type: horizontal)
    public Transform uiCanvasFollowTarget; // character's head, or bow position

    private float chargeTime = 0f;
    private bool isCharging = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && CanAttack())
        {
            StartCharging();
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            Charge();
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseArrow();
        }

        UpdateUIPosition();
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;
        chargeBarUI.gameObject.SetActive(true);
    }

    private void Charge()
    {
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(chargeTime, maxChargeTime);
        float fillAmount = chargeTime / maxChargeTime;
        chargeBarUI.fillAmount = fillAmount;
    }

    private void ReleaseArrow()
    {
        isCharging = false;
        isReloading = true;
        lastAttackTime = Time.time;

        float powerRatio = chargeTime / maxChargeTime;
        float arrowForce = Mathf.Lerp(minArrowForce, maxArrowForce, powerRatio);

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = transform.parent.right;
        //arrow.transform.right = direction;
        rb.AddForce(new Vector2(direction.x, direction.y + 0.5f) * arrowForce, ForceMode2D.Impulse);


        chargeBarUI.gameObject.SetActive(false);
        Invoke(nameof(Reload), reloadTime);
    }

    private void Reload()
    {
        isReloading = false;
    }

    private void UpdateUIPosition()
    {
        if (uiCanvasFollowTarget != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(uiCanvasFollowTarget.position + Vector3.up * 1.5f);
            chargeBarUI.transform.position = screenPos;
        }
    }

    private bool isReloading = false;
}
