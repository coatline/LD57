using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Rigidbody rb;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 9f;
    public float lookSensitivity = 2f;
    public float maxLookAngle = 80f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation;

    private bool isSprinting;

    void OnEnable() => Cursor.lockState = CursorLockMode.Locked;

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();
    public void OnSprint(InputAction.CallbackContext ctx) => isSprinting = ctx.ReadValueAsButton();

    void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 velocity = moveDir.normalized * speed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    void LateUpdate()
    {
        // Rotate horizontally (yaw)
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        // Rotate camera vertically (pitch)
        xRotation -= lookInput.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
