using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Objetivo (el guardia)")]
    public Transform target;

    [Header("Rotación con el mouse")]
    public float mouseSensitivity = 0.15f;

    [Header("Límites verticales")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Límites horizontales (evitar mirar atrás)")]
    public float minYaw = -90f;  
    public float maxYaw = 90f;    

    private float yaw;
    private float pitch;

    void Start()
    {
        if (target == null)
            Debug.LogWarning("FirstPersonCamera: no hay target asignado.");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = 0;    
        pitch = 0;
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        float mouseX = delta.x * mouseSensitivity;
        float mouseY = delta.y * mouseSensitivity;

        yaw += mouseX;     
        pitch -= mouseY;    

       
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

        
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

       
        transform.position = target.position;
    }
}
