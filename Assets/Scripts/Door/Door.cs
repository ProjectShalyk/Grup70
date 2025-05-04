using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorSwitch[] doorSwitches;

    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        for (int i = 0; i < doorSwitches.Length; i++)
        {
            doorSwitches[i].attachedDoor = this;
            doorSwitches[i].index = i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckSwitches()
    {
        bool flag = true;
        int count = doorSwitches.Length;

        for (int i = 0; i < doorSwitches.Length; i++)
        {
            if (doorSwitches[i].state == false)
            {
                flag = false;
                count--;
            }
        }
        float colorLerp = (float) (count / doorSwitches.Length); 

        _spriteRenderer.color = Color.Lerp(Color.red, Color.green, colorLerp);

        Debug.Log("all switches " +  flag);
    }
}
