using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    private List<NPCQueueMovement> npcs = new List<NPCQueueMovement>();

    
    public void RegisterNPC(NPCQueueMovement npc)
    {
        if (!npcs.Contains(npc))
            npcs.Add(npc);
    }

    
    public int GetPointIndexFor(NPCQueueMovement npc)
    {
        if (npcs.Count == 0 || npc.queuePoints == null || npc.queuePoints.Length == 0)
            return 0;

        
        Transform origin = npc.queuePoints[0];

        
        List<NPCQueueMovement> ordered = new List<NPCQueueMovement>(npcs);
        ordered.Sort((a, b) =>
        {
            float da = Vector3.Distance(a.transform.position, origin.position);
            float db = Vector3.Distance(b.transform.position, origin.position);
            return da.CompareTo(db);
        });

        int index = ordered.IndexOf(npc);
        if (index < 0) index = 0;

        
        return Mathf.Min(index, npc.queuePoints.Length - 1);
    }

    
    public void RemoveFromQueue(NPCQueueMovement npc)
    {
        if (npcs.Contains(npc))
            npcs.Remove(npc);
    }

    
    public NPCQueueMovement GetNPCInFrontOf(NPCQueueMovement npc)
    {
        return null;
    }
}
