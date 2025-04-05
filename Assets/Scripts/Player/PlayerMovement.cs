using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] Rigidbody rb;

    Vector2 moveInput;
    bool isSprinting;

    public void SetIsSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 velocity = moveDir.normalized * speed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
}
