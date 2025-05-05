using UnityEngine;

public class UniverseChanger : MonoBehaviour
{
    [Header("Universe Index to Load")]
    [SerializeField] private int universeIndexToLoad;

    [Header("Player Detection")]
    [SerializeField] private float interactionDistance = 3f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("UniverseChanger: Player not found! Make sure the Player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            UniverseManager.Instance.ChangeUniverseByIndex(universeIndexToLoad);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Görsel yardým için etkileþim mesafesini çiz
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
