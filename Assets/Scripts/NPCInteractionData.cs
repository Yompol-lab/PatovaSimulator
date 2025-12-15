using System;
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

    [Header("Puntuación / Flecha")]
    [Tooltip("Si está marcado: este NPC debería SUMAR si lo dejás pasar.")]
    public bool sumaPuntos;

    [Tooltip("Si está marcado: este NPC debería SUMAR si lo RECHAZÁS.")]
    public bool restaPuntos;

    public ReputationArrow arrowController;

    private NPCQueueMovement movement;

    [Header("Documentación Falsa")]
    public bool dniFalso = false;
    public bool entradaFalsa = false;

    [Header("Mesa de Contrabando")]
    public ContrabandTable contrabandTable;

    void Start()
    {
        movement = GetComponent<NPCQueueMovement>();

        if (arrowController == null)
            arrowController = FindObjectOfType<ReputationArrow>();
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
            movement.GoToPoint(rejectExitPoint.position, true);
    }


    public void ApplyDecisionToArrow(bool decisionLetPass)
    {
        if (arrowController == null)
            return;

        bool correctDecision = false;

        if (sumaPuntos)
        {
            correctDecision = decisionLetPass;      
        }
        else if (restaPuntos)
        {
            correctDecision = !decisionLetPass;    
        }
        else
        {
            return;
        }

        if (correctDecision)
            arrowController.ApplyGoodDecision();
        else
            arrowController.ApplyBadDecision();

       
        LogDecisionToFirebase(decisionLetPass, correctDecision);
    }



    [System.Serializable]
    public class DecisionLog
    {
        public string npcName;
        public bool dniFalso;
        public bool entradaFalsa;

        public bool jugadorDejoPasar;    
        public bool decisionCorrecta;     

        public string resultadoTexto;     
        public long timestamp;            
    }

    public void LogDecisionToFirebase(bool decisionLetPass, bool correct)
    {
        DecisionLog log = new DecisionLog
        {
            npcName = npcName,
            dniFalso = dniFalso,
            entradaFalsa = entradaFalsa,

            jugadorDejoPasar = decisionLetPass,
            decisionCorrecta = correct,
            resultadoTexto = correct ? "Correcto" : "Incorrecto",

            
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.SaveDecision(log);
        }

        
        Debug.Log("LOG -> " + JsonUtility.ToJson(log));
    }

    public void DropContrabandOnTable()
    {
        if (!hasDrugs || contrabandPrefabs == null || contrabandPrefabs.Length == 0)
            return;

        if (contrabandTable == null)
        {
            Debug.LogWarning("No hay mesa de contrabando asignada.");
            return;
        }

        foreach (GameObject prefab in contrabandPrefabs)
        {
            contrabandTable.EnqueueContraband(prefab);
        }
    }



}
