using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    private List<NPCQueueMovement> npcs = new List<NPCQueueMovement>();

   
    public void RegisterNPC(NPCQueueMovement npc)
    {
        if (npc == null) return;
        if (npcs.Contains(npc)) return;

        npcs.Add(npc);
    }

   
    public void RemoveFromQueue(NPCQueueMovement npc)
    {
        if (npc == null) return;
        npcs.Remove(npc);
    }

   
    public bool IsRegistered(NPCQueueMovement npc)
    {
        return npcs.Contains(npc);
    }

    
    public int GetPointIndexFor(NPCQueueMovement npc)
    {
        return npcs.IndexOf(npc);
    }

    
    public int Count()
    {
        return npcs.Count;
    }

    
    public int GetNPCCount()
    {
        return npcs.Count;
    }

    public bool HasFreeSlot(int max)
    {
        return npcs.Count < max;
    }
}
