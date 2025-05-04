using UnityEngine;
using TMPro;

public class DoorSwitch : MonoBehaviour, IInteractable
{
    public Door attachedDoor;
    public int index;
    public bool state;

    private GameObject _childObject;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        _childObject = transform.GetChild(0).gameObject;
        _childObject.SetActive(false);
    }

    public void OnAreaEnter()
    {
        _childObject.SetActive(true);
    }

    public void OnAreaExit()
    {
        _childObject.SetActive(false);
    }

    public void OnInteract()
    {
        state = !state;

        if (state)
        {
            _spriteRenderer.color = Color.green;
        }
        else
        {
            _spriteRenderer.color = Color.red;
        }

        attachedDoor.CheckSwitches();
    }

}
