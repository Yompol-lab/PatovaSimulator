using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    private readonly List<NPCQueueMovement> npcs = new List<NPCQueueMovement>();

    public void RegisterNPC(NPCQueueMovement npc)
    {
        if (!npcs.Contains(npc))
            npcs.Add(npc);
    }

    public void RemoveFromQueue(NPCQueueMovement npc)
    {
        if (npcs.Contains(npc))
            npcs.Remove(npc);
    }

    
    public bool IsRegistered(NPCQueueMovement npc)
    {
        return npcs.Contains(npc);
    }

    
    public int GetPointIndexFor(NPCQueueMovement npc)
    {
        if (!npcs.Contains(npc))
            return -1;

        return npcs.IndexOf(npc);
    }

    
    public int GetNPCCount()
    {
        return npcs.Count;
    }

    
    public bool HasFreeSlot(int maxNPCInQueue)
    {
        return npcs.Count < maxNPCInQueue;
    }

   
    public NPCQueueMovement GetNPCInFrontOf(NPCQueueMovement npc)
    {
        int index = npcs.IndexOf(npc);
        if (index > 0)
            return npcs[index - 1];

        return null;
    }
}
