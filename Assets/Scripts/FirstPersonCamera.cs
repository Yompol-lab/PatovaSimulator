using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [HideInInspector] public bool freezeCamera = false;

    [Header("Referencia al cuerpo del jugador")]
    public Transform playerBody;    

    [Header("Rotación con el mouse")]
    public float mouseSensitivity = 0.15f;

    [Header("Límites verticales (mirar arriba/abajo)")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Límites horizontales (evitar mirar atrás)")]
    public float minYaw = -90f;      
    public float maxYaw = 90f;       

    private float yawOffset;        
    private float pitch;
    private float baseBodyYaw;       

    void Start()
    {
        if (playerBody == null)
        {
           
            playerBody = transform.parent;
        }

        if (playerBody == null)
        {
            Debug.LogWarning("FirstPersonCamera: no se asignó playerBody.");
            baseBodyYaw = 0f;
        }
        else
        {
            baseBodyYaw = playerBody.localEulerAngles.y;
        }

        
        pitch = transform.localEulerAngles.x;
        yawOffset = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (freezeCamera)
            return;

        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        float mouseX = delta.x * mouseSensitivity;
        float mouseY = delta.y * mouseSensitivity;

        
        yawOffset += mouseX;
        yawOffset = Mathf.Clamp(yawOffset, minYaw, maxYaw);

        float finalBodyYaw = baseBodyYaw + yawOffset;

        if (playerBody != null)
        {
            Vector3 bodyEuler = playerBody.localEulerAngles;
            bodyEuler.y = finalBodyYaw;
            playerBody.localEulerAngles = bodyEuler;
        }

       
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 camEuler = transform.localEulerAngles;
        camEuler.x = pitch;
        camEuler.y = 0f;   
        camEuler.z = 0f;
        transform.localEulerAngles = camEuler;
    }
}
