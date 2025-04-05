using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FirstPersonCamera firstPersonCamera;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform playerCamera;

    [Header("Settings")]

    private Vector2 lookInput;
    private float xRotation;

    private bool isSprinting;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        playerMovement.SetMoveInput(ctx.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        isSprinting = ctx.ReadValueAsButton();
        playerMovement.SetIsSprinting(isSprinting);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            PauseMenu.I.TogglePause();
    }

    void LateUpdate()
    {
        if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible)
            return;

        firstPersonCamera.RotateCamera(lookInput);
        //// Rotate horizontally (yaw)
        //transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        //// Rotate camera vertically (pitch)
        //xRotation -= lookInput.y * lookSensitivity;
        //xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        //playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
