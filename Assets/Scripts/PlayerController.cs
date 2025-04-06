using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FirstPersonCamera firstPersonCamera;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform playerCamera;
    [SerializeField] PlayerInput playerInput;

    Vector2 lookInputs;

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
        playerMovement.SetIsSprinting(ctx.ReadValueAsButton());
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (Cursor.visible || Cursor.lockState != CursorLockMode.Locked)
            return;

        if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            firstPersonCamera.SetInputValues(ctx.ReadValue<Vector2>() * Time.deltaTime);
            lookInputs = Vector2.zero;
        }
        else
        {
            lookInputs = ctx.ReadValue<Vector2>();
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            PauseMenu.I.TogglePause();
    }

    private void FixedUpdate()
    {
        firstPersonCamera.SetInputValues(lookInputs * Time.fixedDeltaTime);
    }
}
