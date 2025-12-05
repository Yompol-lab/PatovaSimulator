using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] npcPrefabs;
    public Transform spawnPoint;

    public int maxNPCInQueue = 3;
    public float spawnDelay = 2f;

    private NPCQueueManager queueManager;

    void Start()
    {
        queueManager = FindObjectOfType<NPCQueueManager>();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (queueManager == null)
                continue;

            int currentCount = queueManager.GetNPCCount();

            if (currentCount < maxNPCInQueue)
            {
                SpawnNPC();
            }
        }
    }

    void SpawnNPC()
    {
        if (npcPrefabs.Length == 0 || spawnPoint == null) return;

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
