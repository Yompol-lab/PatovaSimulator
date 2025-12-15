using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrabandTable : MonoBehaviour
{
    [Header("Mesa")]
    public Transform dropPoint;
    public Transform slotsParent;

    [Header("Timing")]
    public float spawnDelay = 0.3f;
    public float moveDuration = 0.25f;

    private Queue<GameObject> queue = new Queue<GameObject>();
    private List<Transform> slots = new List<Transform>();
    private int nextSlotIndex = 0;
    private bool processing = false;

    void Awake()
    {
        slots.Clear();

        if (slotsParent == null)
        {
            Debug.LogError("SlotsParent NO asignado en ContrabandTable");
            return;
        }

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots.Add(slotsParent.GetChild(i));
            Debug.Log("Slot cargado: " + slotsParent.GetChild(i).name);
        }
    }

    public void EnqueueContraband(GameObject prefab)
    {
        queue.Enqueue(prefab);

        if (!processing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        processing = true;

        while (queue.Count > 0)
        {
            GameObject prefab = queue.Dequeue();
            SpawnAndPlace(prefab);
            yield return new WaitForSeconds(spawnDelay);
        }

        processing = false;
    }

    private void SpawnAndPlace(GameObject prefab)
    {
        if (slots.Count == 0) return;

        Transform slot = slots[nextSlotIndex];
        nextSlotIndex = (nextSlotIndex + 1) % slots.Count;

        
        GameObject obj = Instantiate(prefab, dropPoint.position, dropPoint.rotation);

        
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; 
        }

        
        StartCoroutine(MoveToSlot(obj, slot));
    }

    private IEnumerator MoveToSlot(GameObject obj, Transform slot)
    {
        Vector3 startPos = obj.transform.position;
        Quaternion startRot = obj.transform.rotation;

        float t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float k = t / moveDuration;

            obj.transform.position = Vector3.Lerp(startPos, slot.position, k);
            obj.transform.rotation = Quaternion.Slerp(startRot, slot.rotation, k);
            yield return null;
        }

        
        obj.transform.position = slot.position;
        obj.transform.rotation = slot.rotation;

        
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }

    
    void OnDrawGizmos()
    {
        if (slotsParent == null) return;

        Gizmos.color = Color.green;
        foreach (Transform t in slotsParent)
        {
            Gizmos.DrawSphere(t.position, 0.03f);
        }
    }
}
