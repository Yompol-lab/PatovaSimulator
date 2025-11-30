using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    private List<NPCQueueMovement> npcs = new List<NPCQueueMovement>();

    public void RegisterNPC(NPCQueueMovement npc)
    {
        npcs.Add(npc);
    }

    public NPCQueueMovement GetNPCInFrontOf(NPCQueueMovement npc)
    {
        
        List<NPCQueueMovement> ordered = new List<NPCQueueMovement>(npcs);

        ordered.Sort((a, b) =>
        {
            if (a.currentPointIndex == b.currentPointIndex)
            {
                float distA = Vector3.Distance(a.transform.position, npc.queuePoints[a.currentPointIndex].position);
                float distB = Vector3.Distance(b.transform.position, npc.queuePoints[b.currentPointIndex].position);
                return distA.CompareTo(distB);
            }

            return a.currentPointIndex.CompareTo(b.currentPointIndex);
        });

        int index = ordered.IndexOf(npc);
        if (index > 0)
            return ordered[index - 1];

        return null;
    }
}
