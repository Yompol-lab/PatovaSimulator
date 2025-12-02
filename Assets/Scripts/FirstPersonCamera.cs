using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [HideInInspector] public bool freezeCamera = false;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        Vector3 rot = transform.localRotation.eulerAngles;
        pitch = rot.x;
        yaw = rot.y;
    }

    void LateUpdate()
    {
        if (freezeCamera)
            return;

        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        float mouseX = delta.x * mouseSensitivity;
        float mouseY = delta.y * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);

        
    }
}
