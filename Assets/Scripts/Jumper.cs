using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] float minJumpDelay;
    [SerializeField] float jumpForce;
    [SerializeField] Rigidbody rb;

    IntervalTimer jumpReloadTimer;
    bool cantJump;

    private void Awake()
    {
        jumpReloadTimer = new IntervalTimer(minJumpDelay);
    }

    public void TryJump()
    {
        if (cantJump)
            return;

        if (groundDetector.IsOnGround)
            Jump();
    }

    void Update()
    {
        if (jumpReloadTimer.DecrementIfRunning(Time.deltaTime))
            cantJump = false;
    }

    void Jump()
    {
        cantJump = true;
        jumpReloadTimer.Start();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
