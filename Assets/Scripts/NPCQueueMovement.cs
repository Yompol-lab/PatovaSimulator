using UnityEngine;

public class NPCQueueMovement : MonoBehaviour
{
    [Header("Punto de entrada a la fila")]
    public Transform entryPoint;

    [Header("Puntos de la fila (VIP o normal)")]
    public Transform[] queuePoints;

    [Header("A quién mira el primero de la fila")]
    public Transform lookTarget;

    [Header("Movimiento")]
    public float speed = 2f;
    public float stopDistance = 0.25f;
    public float entryStopDistance = 0.15f;

    [HideInInspector] public int currentPointIndex = 0;

    private NPCQueueManager manager;
    private Animator animator;
    private Vector3 lastPosition;

    
    [HideInInspector] public bool ignoreQueueMovement = false;
    private bool goingToExternalPoint = false;
    private Vector3 externalTarget;
    private bool destroyOnArrival = false;

    
    public bool IsFirstInLine
    {
        get
        {
            if (manager == null) return false;
            return manager.GetPointIndexFor(this) == 0;
        }
    }

    void Start()
    {
        manager = FindObjectOfType<NPCQueueManager>();
        animator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
       
        if (ignoreQueueMovement)
        {
            if (goingToExternalPoint)
            {
                MoveToExternalPoint();
                UpdateAnimation();
            }
            return;
        }

        
        if (!IsInQueue())
        {
            MoveTowardsEntry();
            UpdateAnimation();
            return;
        }

        
        currentPointIndex = Mathf.Clamp(manager.GetPointIndexFor(this), 0, queuePoints.Length - 1);
        MoveTowardsQueuePoint();
        UpdateAnimation();
    }

    bool IsInQueue()
    {
        return manager != null && manager.IsRegistered(this);
    }

    
    void MoveTowardsQueuePoint()
    {
        if (currentPointIndex < 0 || currentPointIndex >= queuePoints.Length)
            return;

        Transform point = queuePoints[currentPointIndex];
        if (point == null) return;

        Vector3 target = point.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;

        bool isFront = currentPointIndex == 0;

        if (dist > stopDistance)
        {
            MoveStep(dir, dist);
        }

        
        if (isFront && dist <= stopDistance && lookTarget != null)
        {
            Vector3 lookDir = lookTarget.position - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 360f * Time.deltaTime);
            }
        }
    }

  
    void MoveTowardsEntry()
    {
        if (entryPoint == null || manager == null) return;

        Vector3 target = entryPoint.position;
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        float dist = dir.magnitude;

        if (dist <= entryStopDistance)
        {
            manager.RegisterNPC(this);
            return;
        }

        MoveStep(dir, dist);
    }

    
    public void GoToPoint(Vector3 point, bool destroyAfter)
    {
        ignoreQueueMovement = true;
        goingToExternalPoint = true;
        externalTarget = point;
        destroyOnArrival = destroyAfter;

        if (manager != null)
            manager.RemoveFromQueue(this);
    }

    void MoveToExternalPoint()
    {
        Vector3 target = externalTarget;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;

        if (dist <= 0.05f)
        {
            goingToExternalPoint = false;
            animator.SetBool("IsWalking", false);

            if (destroyOnArrival)
                Destroy(gameObject);

            return;
        }

        MoveStep(dir, dist);
    }

    
    void MoveStep(Vector3 dir, float dist)
    {
        Vector3 step = dir.normalized * speed * Time.deltaTime;
        if (step.magnitude > dist)
            step = dir.normalized * dist;

        transform.position += step;

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 180f * Time.deltaTime);
        }
    }

    
    void UpdateAnimation()
    {
        if (animator == null) return;

        Vector3 displacement = transform.position - lastPosition;
        bool moving = displacement.sqrMagnitude > 0.0001f;

        animator.SetBool("IsWalking", moving);
        lastPosition = transform.position;
    }
}
