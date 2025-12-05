using System.Collections;
using UnityEngine;

public class NPCSpawnerPool : MonoBehaviour
{
    [Header("Referencia a la cola")]
    public NPCQueueManager queueManager;

    [Header("Spawn")]
    public float spawnInterval = 5f;     
    public int maxNPCInQueue = 3;        

    
    NPCQueueMovement[] npcPool;
    int nextIndex = 0;

    void Awake()
    {
        
        npcPool = GetComponentsInChildren<NPCQueueMovement>(true);

        if (npcPool == null || npcPool.Length == 0)
        {
            Debug.LogError("NPCSpawnerPool: no encontré NPCQueueMovement como hijos de " + gameObject.name);
        }
        else
        {
            Debug.Log("NPCSpawnerPool: pool armado con " + npcPool.Length + " NPCs.");
        }
    }

    void Start()
    {
        if (queueManager == null)
            queueManager = FindObjectOfType<NPCQueueManager>();

        if (queueManager == null)
        {
            Debug.LogError("NPCSpawnerPool: no encontré NPCQueueManager en la escena.");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

           
            if (queueManager == null || npcPool == null || npcPool.Length == 0)
                continue;

            
            if (!queueManager.HasFreeSlot(maxNPCInQueue))
                continue;

            ActivateNextNPC();
        }
    }

    void ActivateNextNPC()
    {
        if (npcPool == null || npcPool.Length == 0)
            return;

        int safety = npcPool.Length;

        while (safety-- > 0)
        {
            NPCQueueMovement npc = npcPool[nextIndex];
            nextIndex = (nextIndex + 1) % npcPool.Length;

            if (npc == null)
                continue;

            
            if (!npc.gameObject.activeSelf)
            {
                npc.gameObject.SetActive(true);
                Debug.Log("Spawner: activé NPC " + npc.name);
                return;
            }
        }

       
    }
}
