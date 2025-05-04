using UnityEngine;

public partial class PlayerController
{
    void InteractUpdate()
    {
        if (currentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentInteractable.OnInteract();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable inteactable;
        if (collision.TryGetComponent<IInteractable>(out inteactable))
        {
            inteactable.OnAreaEnter();
            currentInteractable = inteactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable inteactable;
        if (collision.TryGetComponent<IInteractable>(out inteactable))
        {
            inteactable.OnAreaExit();
            currentInteractable = null;
        }
    }

}