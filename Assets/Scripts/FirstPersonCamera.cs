using UnityEngine;
using UnityEngine.InputSystem;   

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Objetivo (el guardia)")]
    public Transform target;         

    [Header("Rotación con el mouse")]
    public float mouseSensitivity = 0.15f;
    public float minPitch = -60f;    
    public float maxPitch = 60f;     

    private float yaw;                
    private float pitch;              

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("FirstPersonCamera: no hay target asignado.");
        }

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
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

        
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        
        transform.position = target.position;
    }
}
