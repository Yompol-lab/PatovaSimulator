using UnityEngine;

public class NPCQueueMovement : MonoBehaviour
{
    [Header("Puntos de la fila (VIP o Normal)")]
    public Transform[] queuePoints;

    [Header("Movimiento")]
    public float speed = 2f;
    public float stopDistance = 1.2f;

    [HideInInspector]
    public int currentPointIndex = 0;

    private NPCQueueManager manager;
    private Animator animator;

    
    private Vector3 lastPosition;

    private void Start()
    {
        currentPointIndex = 0;

        manager = FindObjectOfType<NPCQueueManager>();
        if (manager != null)
            manager.RegisterNPC(this);
        else
            Debug.LogWarning("No se encontró NPCQueueManager en la escena.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("NPCQueueMovement: no se encontró Animator.");

        lastPosition = transform.position;
    }

    private void Update()
    {
        

        if (currentPointIndex < queuePoints.Length)
        {
            NPCQueueMovement npcAhead = manager != null ? manager.GetNPCInFrontOf(this) : null;

            bool canMove = false;

            if (npcAhead == null)
            {
                canMove = true; 
            }
            else
            {
                float dist = Vector3.Distance(transform.position, npcAhead.transform.position);
                canMove = dist > stopDistance;
            }

            if (canMove)
            {
                MoveTowardsPoint();
            }
        }

        

        UpdateAnimationByMovement();
    }

    private void MoveTowardsPoint()
    {
        if (currentPointIndex >= queuePoints.Length)
            return;

        Vector3 target = queuePoints[currentPointIndex].position;
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        
        if (dir.magnitude < 0.05f)
        {
            currentPointIndex++;
            return;
        }

        transform.position += dir.normalized * speed * Time.deltaTime;
        transform.forward = dir.normalized;
    }

    private void UpdateAnimationByMovement()
    {
        if (animator == null) return;

        Vector3 displacement = transform.position - lastPosition;
        float sqrMag = displacement.sqrMagnitude;

       
        bool isWalking = sqrMag > 0.0001f;

        animator.SetBool("IsWalking", isWalking);

        lastPosition = transform.position;
    }
}
