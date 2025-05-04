using UnityEngine;
using UnityEngine.UI;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float maxArrowForce = 30f;
    public float minArrowForce = 5f;
    public float maxChargeTime = 2f;

    public float reloadTime = 1f;
    public Image reloadBarUI;
    private float reloadTimer = 0f;


    public Image chargeBarUI; 
    public Transform uiCanvasFollowTarget; 

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

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            float reloadProgress = Mathf.Clamp01(reloadTimer / reloadTime);
            reloadBarUI.fillAmount = reloadProgress;
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
        reloadTimer = 0f; // reload sayacýný baþlat
        reloadBarUI.fillAmount = 0f;
        reloadBarUI.gameObject.SetActive(true);
        lastAttackTime = Time.time;

        float powerRatio = chargeTime / maxChargeTime;
        float arrowForce = Mathf.Lerp(minArrowForce, maxArrowForce, powerRatio);

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = transform.parent.right;
        rb.AddForce(new Vector2(direction.x, direction.y) * arrowForce, ForceMode2D.Impulse);
        arrow.GetComponent<Arrow>().damage = damage;

        chargeBarUI.gameObject.SetActive(false);
        Invoke(nameof(Reload), reloadTime);
    }


    private void Reload()
    {
        isReloading = false;
        reloadBarUI.gameObject.SetActive(false);
    }


    private void UpdateUIPosition()
    {
        if (uiCanvasFollowTarget != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(uiCanvasFollowTarget.position + Vector3.up * 1.5f);
            chargeBarUI.transform.position = screenPos;
        }
        if (uiCanvasFollowTarget != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(uiCanvasFollowTarget.position + Vector3.up * 1.5f);
            chargeBarUI.transform.position = screenPos;
            reloadBarUI.transform.position = screenPos + Vector2.down * 20f;
        }
    }

    private bool isReloading = false;
}
