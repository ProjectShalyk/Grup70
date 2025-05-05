using System.Collections;
using UnityEngine;

public class UniverseManager : MonoBehaviour
{
    [Header("Temp Universe Control")]
    [SerializeField] private GameObject[] universes;
    private UniverseController[] universeControllers;
    int universeIndex = 0;

    public static UniverseManager Instance { get; private set; }
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
    void Start()
    {
        universeControllers = new UniverseController[universes.Length];
        for (int i = 0; i < universes.Length; i++)
        {
            universeControllers[i] = universes[i].GetComponent<UniverseController>();
            universeControllers[i].index = i;
            if (i == 0)
            {
                StartCoroutine(universeControllers[i].UniverseFadeIn());
            }
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) // geçici
        //{
        //    ChangeUniverse();
        //}
    }

    public void ChangeUniverse()
    {
        StartCoroutine(universeControllers[universeIndex].UniverseFadeOut());
        universeIndex++;
        if (universeIndex >= universes.Length)
        {
            universeIndex = 0;
        }
        StartCoroutine(universeControllers[universeIndex].UniverseFadeIn());
        //universes[universeIndex].SetActive(true);
    }

    public void ChangeUniverseByIndex(int index)
    {
        if (index < 0 || index >= universes.Length)
        {
            Debug.LogWarning("ChangeUniverseByIndex: Invalid index " + index);
            return;
        }

        StartCoroutine(ChangeUniverseCoroutine(index));
    }

    private IEnumerator ChangeUniverseCoroutine(int newIndex)
    {
        yield return StartCoroutine(universeControllers[universeIndex].UniverseFadeOut());

        universeIndex = newIndex;

        yield return StartCoroutine(universeControllers[universeIndex].UniverseFadeIn());
    }

}
