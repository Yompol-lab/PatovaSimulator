using UnityEngine;
using UnityEngine.InputSystem;

public class MovementLimiter : MonoBehaviour
{
    [Header("Velocidad")]
    public float moveSpeed = 5f;

    [Header("Límites del Mundo")]
    public float minX = 36.541f;
    public float maxX = 38.169f;
    public float minZ = -7.929f;
    public float maxZ = -6.921f;

    private Vector2 moveInput;

    void Update()
    {
        
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float h = 0f;
        float v = 0f;

        if (kb.aKey.isPressed) h = -1;
        if (kb.dKey.isPressed) h = 1;
        if (kb.wKey.isPressed) v = 1;
        if (kb.sKey.isPressed) v = -1;

        moveInput = new Vector2(h, v);

        
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        
        Vector3 newPos = transform.position + direction * moveSpeed * Time.deltaTime;

        
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);

        
        transform.position = newPos;
    }
}
