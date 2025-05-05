using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public PlayerController player;

    [SerializeField] private GameObject lastCheckpoint;

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
           RespawnPlayer();
        }
    }

    public void SetCheckpoint(GameObject gameObject)
    {
        lastCheckpoint = gameObject;
    }

    public GameObject GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void SetPLayerFalse()
    {
        player.gameObject.SetActive(false);
    }

    public void RespawnPlayer()
    {
        player.gameObject.SetActive(true);
        player.StartRespawn();
    }


}
