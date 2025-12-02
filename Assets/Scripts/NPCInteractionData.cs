using UnityEngine;

public class NPCInteractionData : MonoBehaviour
{
    [Header("Identidad")]
    public string npcName;
    public string dni;
    public Sprite dniImagen;

    [Header("Estado")]
    public bool hasDrugs = false;
    public bool badBehavior = false;
    public bool dressCodeBad = false;

    [Header("Puntos de movimiento")]
    public Transform insideClubPoint;
    public Transform rejectExitPoint;

    private NPCQueueMovement movement;

    void Start()
    {
        movement = GetComponent<NPCQueueMovement>();
    }

    public void ShowDNI()
    {
        Debug.Log("Mostrando DNI: " + npcName);
    }

    public void PerformGuardCheck()
    {
        Debug.Log(hasDrugs ? npcName + " TIENE MERCA" : npcName + " está limpio");
    }

    public void CheckDressCode()
    {
        Debug.Log(dressCodeBad ? npcName + " NO cumple vestimenta" : npcName + " está bien vestido");
    }

    public void CheckBehavior()
    {
        Debug.Log(badBehavior ? npcName + " se comporta mal" : npcName + " normal");
    }

    
    public void EnterClub()
    {
        if (movement != null && insideClubPoint != null)
        {
            movement.GoToPoint(insideClubPoint.position, true);
        }
    }

   
    public void LeaveClub()
    {
        if (movement != null && rejectExitPoint != null)
        {
            movement.GoToPoint(rejectExitPoint.position, true);
        }
    }
}
