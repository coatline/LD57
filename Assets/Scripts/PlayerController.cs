using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FirstPersonCamera firstPersonCamera;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform playerCamera;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] FirstPersonInteractor interactor;
    [SerializeField] ObjectHolder objectHolder;
    [SerializeField] Jumper jumper;

    Vector2 lookInputs;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Enable()
    {
        firstPersonCamera.SetCurrentLookingPosition(new Vector2(firstPersonCamera.transform.eulerAngles.y, 0));
        playerInput.enabled = true;
        enabled = true;
    }

    public void Disable()
    {
        playerInput.enabled = false;
        enabled = false;
    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            objectHolder.TryDropItem();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            jumper.TryJump();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            interactor.TryInteract();
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
            lookInputs = ctx.ReadValue<Vector2>() * 35f;
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            PauseMenu.I.TogglePause();
    }

    private void FixedUpdate()
    {
        if (playerInput.currentControlScheme == "Keyboard&Mouse" == false)
            firstPersonCamera.SetInputValues(lookInputs * Time.fixedDeltaTime);
    }
}
