using UnityEngine;

public class NPCInteractionData : MonoBehaviour
{
    [Header("Identidad")]
    public string npcName;
    public string dni;

    public Sprite dniImagen;
    public Sprite ticketImagen;

    [Header("Estado")]
    public bool hasDrugs = false;
    public bool badBehavior = false;
    public bool dressCodeBad = false;

    [Header("Puntos de movimiento")]
    public Transform insideClubPoint;
    public Transform rejectExitPoint;

    [Header("Contrabando")]
   
    public GameObject[] contrabandPrefabs;
    public Transform contrabandSpawnPoint;
    
    [HideInInspector] public GameObject[] spawnedContraband;

    private NPCQueueMovement movement;

    void Start()
    {
        movement = GetComponent<NPCQueueMovement>();
    }

    public void ShowDNI()
    {
        Debug.Log("DNI de " + npcName + ": " + dni);
    }

    public void PerformGuardCheck()
    {
        if (hasDrugs)
            Debug.Log(npcName + " TIENE merca encima");
        else
            Debug.Log(npcName + " está limpio");
    }

    public void CheckDressCode()
    {
        if (dressCodeBad)
            Debug.Log(npcName + " NO cumple el código de vestimenta");
        else
            Debug.Log(npcName + " está bien vestido");
    }

    public void CheckBehavior()
    {
        if (badBehavior)
            Debug.Log(npcName + " se comporta mal / dudoso");
        else
            Debug.Log(npcName + " se comporta normal");
    }

    public void EnterClub()
    {
        if (movement != null && insideClubPoint != null)
            movement.GoToPoint(insideClubPoint.position, true);
    }

    public void LeaveClub()
    {
        if (movement != null && rejectExitPoint != null)
            movement.GoToPoint(rejectExitPoint.position, false);
    }
}
