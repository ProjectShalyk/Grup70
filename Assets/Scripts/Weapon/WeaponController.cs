using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform hand;  // Ýlk silahýn transformu
    public Transform characterBody; // Karakterin gövdesi (flip için kullanýlýr)



    void Update()
    {
        Aim();
    }

    void Aim()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - hand.position;
        direction.z = 0f;

        // Açýyý hesapla
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Silahlarý döndür
        hand.rotation = Quaternion.Euler(0, 0, angle);

        // Eðer fare sol taraftaysa, silahlarý aynala (scale ile)
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
