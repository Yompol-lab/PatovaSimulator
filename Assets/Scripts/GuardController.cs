using System.Collections;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public float stopDistance = 0.2f;

    [Header("Puntos")]
    public Transform homePoint;         
    public Transform lookTarget;        

    [Header("Animación")]
    public Animator animator;
    public string isWalkingParam = "IsWalking"; 
    public string searchTrigger = "Search";    
    public float searchDuration = 2f;          

    private Coroutine currentRoutine;

    void Start()
    {
        if (homePoint == null)
        {
            
            GameObject home = new GameObject(name + "_Home");
            home.transform.position = transform.position;
            home.transform.rotation = transform.rotation;
            homePoint = home.transform;
        }

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    
    public void StartCheck(NPCInteractionData npcData, bool hasDrugs)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(CheckRoutine(npcData, hasDrugs));
    }

    private IEnumerator CheckRoutine(NPCInteractionData npcData, bool hasDrugs)
    {
        Transform npc = npcData.transform;

        
        Vector3 checkPos = npc.position - npc.forward * 0.6f;
        checkPos.y = transform.position.y;

        
        yield return MoveTo(checkPos, npc.position);

        
        if (animator != null && !string.IsNullOrEmpty(searchTrigger))
        {
            animator.SetBool(isWalkingParam, false);
            animator.SetTrigger(searchTrigger);
        }

        yield return new WaitForSeconds(searchDuration);

        
        if (hasDrugs && npcData.contrabandPrefab != null)
        {
            
            if (npcData.spawnedContraband == null)
            {
                Transform spawn = npcData.contrabandSpawnPoint != null
                    ? npcData.contrabandSpawnPoint
                    : npc;

                npcData.spawnedContraband = GameObject.Instantiate(
                    npcData.contrabandPrefab,
                    spawn.position + spawn.right * 0.3f,
                    Quaternion.identity
                );
            }
        }

        // 4) Volver a casa
        yield return MoveTo(homePoint.position, lookTarget != null ? lookTarget.position : transform.position);

        currentRoutine = null;
    }

    private IEnumerator MoveTo(Vector3 targetPos, Vector3 lookAtPos)
    {
        while (true)
        {
            Vector3 dir = targetPos - transform.position;
            dir.y = 0f;
            float dist = dir.magnitude;

            if (dist <= stopDistance)
                break;

            Vector3 step = dir.normalized * speed * Time.deltaTime;
            if (step.magnitude > dist)
                step = dir.normalized * dist;

            transform.position += step;

            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    rot,
                    360f * Time.deltaTime
                );
            }

            if (animator != null)
                animator.SetBool(isWalkingParam, true);

            yield return null;
        }

        
        Vector3 lookDir = lookAtPos - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.0001f)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = rot;
        }

        if (animator != null)
            animator.SetBool(isWalkingParam, false);
    }
}
