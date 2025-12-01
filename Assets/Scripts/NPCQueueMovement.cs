using UnityEngine;

public class NPCQueueMovement : MonoBehaviour
{
    [Header("Punto de entrada a la fila (entre las vallas)")]
    public Transform entryPoint;

    [Header("Puntos de la fila (VIP o Normal)")]
    public Transform[] queuePoints; 

    [Header("A quién miran cuando están primeros")]
    public Transform lookTarget;  

    [Header("Movimiento")]
    public float speed = 2f;
    public float stopDistance = 0.25f;      
    public float entryStopDistance = 0.15f;

    [HideInInspector]
    public int currentPointIndex = 0;

    private NPCQueueManager manager;
    private Animator animator;
    private Vector3 lastPosition;

    
    private bool inQueue = false;

    private void Start()
    {
        manager = FindObjectOfType<NPCQueueManager>();
        if (manager == null)
            Debug.LogWarning("NPCQueueMovement: no se encontró NPCQueueManager.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("NPCQueueMovement: no se encontró Animator.");

        lastPosition = transform.position;
    }

    private void Update()
    {
        
        if (!inQueue)
        {
            MoveTowardsEntry();
            UpdateAnimationByMovement();
            return;
        }

        
        if (manager == null || queuePoints == null || queuePoints.Length == 0)
        {
            UpdateAnimationByMovement();
            return;
        }

        currentPointIndex = manager.GetPointIndexFor(this);

        MoveTowardsCurrentQueuePoint();
        UpdateAnimationByMovement();
    }

    
    private void MoveTowardsEntry()
    {
        if (entryPoint == null)
            return;

        Vector3 target = entryPoint.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;

       
        if (dist <= entryStopDistance)
        {
            inQueue = true;

            if (manager != null)
                manager.RegisterNPC(this);   

            return;
        }

        
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
                180f * Time.deltaTime
            );
        }
    }

   
    private void MoveTowardsCurrentQueuePoint()
    {
        if (currentPointIndex < 0 || currentPointIndex >= queuePoints.Length)
            return;

        Transform point = queuePoints[currentPointIndex];
        if (point == null) return;

        Vector3 target = point.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;

        bool isFrontOfQueue = (currentPointIndex == 0);

        
        if (dist > stopDistance)
        {
            Vector3 step = dir.normalized * speed * Time.deltaTime;
            if (step.magnitude > dist)
                step = dir.normalized * dist;

            transform.position += step;

            // Rotación mientras camina
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion walkRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    walkRot,
                    180f * Time.deltaTime
                );
            }
        }

        
        if (isFrontOfQueue && dist <= stopDistance && lookTarget != null)
        {
            Vector3 lookDir = lookTarget.position - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    lookRot,
                    360f * Time.deltaTime
                );
            }
        }
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

    
    public void LeaveQueue()
    {
        if (manager != null)
            manager.RemoveFromQueue(this);

        enabled = false;
    }
}
