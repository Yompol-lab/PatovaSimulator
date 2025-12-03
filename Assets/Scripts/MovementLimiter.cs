using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementLimiter : MonoBehaviour
{
    [Header("Límites del Mundo")]
    public float minX = 36.541f;
    public float maxX = 38.169f;
    public float minZ = -7.929f;
    public float maxZ = -6.921f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        
        controller.enabled = false;      
        transform.position = pos;
        controller.enabled = true;
    }
}
