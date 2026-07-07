using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class FirstPersonPlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera = null;
    [SerializeField, Min(0f)] private float moveSpeed = 4f;
    [SerializeField, Min(0f)] private float mouseSensitivity = 0.12f;

    private float pitch;

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Keyboard keyboard = Keyboard.current;

        Vector2 moveInput = ReadMoveInput(keyboard);
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void Look()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || playerCamera == null)
        {
            return;
        }

        Vector2 lookInput = mouse.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(Vector3.up, lookInput.x);

        pitch = Mathf.Clamp(pitch - lookInput.y, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private static Vector2 ReadMoveInput(Keyboard keyboard)
    {
        if (keyboard == null)
        {
            return Vector2.zero;
        }

        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            input.x -= 1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            input.x += 1f;
        }

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            input.y += 1f;
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            input.y -= 1f;
        }

        return input.sqrMagnitude > 1f ? input.normalized : input;
    }
}
