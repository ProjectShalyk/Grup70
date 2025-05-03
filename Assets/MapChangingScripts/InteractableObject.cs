using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private bool showPromptWhenNear = true;
    
    [Header("Optional UI")]
    [SerializeField] private GameObject interactionPrompt;
    
    private bool playerInRange = false;
    private Transform player;
    
    private void Start()
    {
        // Find the player - assumes player has a tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Hide prompt if it exists
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    private void Update()
    {
        CheckDistance();
        CheckForInteraction();
    }
    
    private void CheckDistance()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            playerInRange = distance <= interactionDistance;
            
            // Show or hide interaction prompt based on distance
            if (interactionPrompt != null && showPromptWhenNear)
                interactionPrompt.SetActive(playerInRange);
        }
    }
    
    private void CheckForInteraction()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }
    
    // This is the function that will be called when the player interacts with the object
    public virtual void Interact()
    {
        Debug.Log($"Player interacted with {gameObject.name}");
        // Override this in derived classes to implement specific behavior
    }
    
    // Optional: Draw the interaction range in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}