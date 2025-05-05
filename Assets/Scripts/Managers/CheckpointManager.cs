using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro kullanýyorsan gerekli

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public PlayerController player;

    [SerializeField] private GameObject lastCheckpoint;
    [SerializeField] private GameObject respawnText; // UI Panel ya da Text objesi
    int lastIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isDead && !player.isRespawning)
        {
            respawnText.SetActive(false); // UI'yi gizle
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(GameObject gameObject)
    {
        lastCheckpoint = gameObject;
        lastIndex = lastCheckpoint.GetComponentInParent<UniverseController>().index;
    }

    public GameObject GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void SetPLayerFalse()
    {
        player.gameObject.SetActive(false);
        respawnText.SetActive(true); // Oyuncu öldüðünde mesajý göster
    }

    public void RespawnPlayer()
    {
        UniverseManager.Instance.ChangeUniverseByIndex(lastIndex);
        player.gameObject.SetActive(true);
        player.StartRespawn();
    }
}
