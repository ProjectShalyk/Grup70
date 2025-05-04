using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform hand;  // �lk silah�n transformu
    public Transform characterBody; // Karakterin g�vdesi (flip i�in kullan�l�r)



    void Update()
    {
        Aim();
    }

    void Aim()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - hand.position;
        direction.z = 0f;

        // A��y� hesapla
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Silahlar� d�nd�r
        hand.rotation = Quaternion.Euler(0, 0, angle);

        // E�er fare sol taraftaysa, silahlar� aynala (scale ile)
        if (mouseWorldPos.x < characterBody.position.x)
        {
            hand.localScale = new Vector3(1, -1, 1);  // Y ekseninde aynala
        }
        else
        {
            hand.localScale = new Vector3(1, 1, 1);
        }
    }
}
