using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform hand;
    public Transform characterBody;
    public Weapon[] weapons;
    private int currentWeaponIndex = 0;

    void Update()
    {
        Aim();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon(); 
        }
    }

    void ChangeWeapon()
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    void Aim()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - hand.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        hand.rotation = Quaternion.Euler(0, 0, angle);

        if (mouseWorldPos.x < characterBody.position.x)
        {
            hand.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            hand.localScale = new Vector3(1, 1, 1);
        }
    }
}
