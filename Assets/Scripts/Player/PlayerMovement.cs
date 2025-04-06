using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public event System.Action<bool> SprintingChanged;
    public event System.Action StaminaFull;

    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration;
    [SerializeField] float maxStamina;
    [SerializeField] Image staminaBar;
    [SerializeField] Rigidbody rb;

    float targetSpeed;
    float currentSpeed;
    Vector2 moveInput;
    bool isSprinting;
    float stamina;


    private void Awake()
    {
        currentSpeed = walkSpeed;
        targetSpeed = currentSpeed;
        stamina = maxStamina;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    public void SetIsSprinting(bool isSprinting)
    {
        if (isSprinting)
            targetSpeed = sprintSpeed;
        else
            targetSpeed = walkSpeed;

        this.isSprinting = isSprinting;
        SprintingChanged?.Invoke(isSprinting);
    }

    private void FixedUpdate()
    {
        if (isSprinting)
        {
            stamina -= Time.deltaTime;

            if (stamina <= 0)
                SetIsSprinting(false);
        }
        else if (stamina < maxStamina)
            stamina += Time.deltaTime / 6f;
        else if (stamina > maxStamina)
        {
            stamina = maxStamina;
            StaminaFull?.Invoke();
        }

        staminaBar.fillAmount = stamina / maxStamina;

        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration);

        Vector3 velocity = moveDir.normalized * currentSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
}
