using UnityEngine;
using UnityEngine.InputSystem;   

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementFPS : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cameraTransform;   

    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float gravity = -9.8f;
    public float jumpForce = 5f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("No hay CharacterController en el Player.");
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        
        float h = 0f;
        float v = 0f;

        if (Keyboard.current.aKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed) h += 1f;
        if (Keyboard.current.sKey.isPressed) v -= 1f;
        if (Keyboard.current.wKey.isPressed) v += 1f;

        Vector2 input = new Vector2(h, v);
        if (input.sqrMagnitude > 1f)
            input = input.normalized;

        
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;
        move *= moveSpeed;

       
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                verticalVelocity = jumpForce;
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

       
        controller.Move(move * Time.deltaTime);

        
        Vector3 flatCamForward = cameraTransform.forward;
        flatCamForward.y = 0f;
        if (flatCamForward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatCamForward);
    }
}
