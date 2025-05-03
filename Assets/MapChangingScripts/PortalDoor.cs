using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PortalDoor : InteractableObject
{
    [SerializeField] private string tagToHide = "MapLayerMain";
    [SerializeField] private string tagToShow = "MapLayerSecondary";
    
    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnStart = false;
    [SerializeField] private float triggerDelay = 5f;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    [SerializeField] private bool useKeyTrigger = true;
    
    // Object references 
    [SerializeField] private List<GameObject> objectsToHide = new List<GameObject>();
    [SerializeField] private List<GameObject> objectsToShow = new List<GameObject>();
    
    [Header("Debug")]
    [SerializeField] private bool manualAssignment = false;
    
    private void Awake()
    {
        if (!manualAssignment)
        {
            // Try to find objects by tag
            FindTaggedObjects();
        }
        else
        {
            // If using manual assignment, check if lists are populated
            Debug.Log("Using manually assigned objects: " + 
                     objectsToHide.Count + " to hide, " + 
                     objectsToShow.Count + " to show");
        }
        
        // Print all available tags in the project for debugging
        Debug.Log("All available tags in project: " + string.Join(", ", UnityEditorInternal.InternalEditorUtility.tags));
        
        // Print all objects in the current scene for debugging
        Debug.Log("--- All GameObjects in current scene ---");
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in allObjects)
        {
            PrintGameObjectAndChildren(obj, 0);
        }
        Debug.Log("--- End of scene objects list ---");
    }
    
    private void PrintGameObjectAndChildren(GameObject obj, int depth)
    {
        string indent = new string('-', depth * 2);
        Debug.Log(indent + obj.name + " | Active: " + obj.activeSelf + " | Tag: " + obj.tag);
        
        foreach (Transform child in obj.transform)
        {
            PrintGameObjectAndChildren(child.gameObject, depth + 1);
        }
    }
    
    // private void Start()
    // {
    //     if (triggerOnStart)
    //     {
    //         Invoke("SwitchVisibility", triggerDelay);
    //     }
    // }
    //
    // private void Update()
    // {
    //     if (useKeyTrigger && Input.GetKeyDown(triggerKey))
    //     {
    //         SwitchVisibility();
    //     }
    // }
    
    private void FindTaggedObjects()
    {
        // Clear existing lists
        objectsToHide.Clear();
        objectsToShow.Clear();
        
        // Get all objects in the scene (active and inactive)
        GameObject[] sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        
        foreach (GameObject root in sceneObjects)
        {
            // Check this object and all its children
            SearchGameObjectAndChildren(root);
        }
        
        Debug.Log("Found " + objectsToHide.Count + " objects with tag '" + tagToHide + "'");
        Debug.Log("Found " + objectsToShow.Count + " objects with tag '" + tagToShow + "'");
    }
    
    
// Override the base Interact method
    public override void Interact()
    {
        base.Interact(); // Call the base method for logging

        SwitchVisibility();
    }
    
    private void SearchGameObjectAndChildren(GameObject obj)
    {
        // Check this object
        if (obj.CompareTag(tagToHide))
        {
            objectsToHide.Add(obj);
            Debug.Log("Found object to hide: " + obj.name);
        }
        else if (obj.CompareTag(tagToShow))
        {
            objectsToShow.Add(obj);
            Debug.Log("Found object to show: " + obj.name);
        }
        
        // Check all children recursively
        foreach (Transform child in obj.transform)
        {
            SearchGameObjectAndChildren(child.gameObject);
        }
    }
    
    public void SwitchVisibility()
    {
        // Hide objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log("Hiding: " + obj.name);
            }
        }

        // Show objects
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log("Showing: " + obj.name);
            }
        }
        
        Debug.Log("Visibility switch complete");
    }
}