using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float minJumpDelay;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Rigidbody rb;

    [Header("Raycasts")]
    [SerializeField] float raycastDepth;
    [SerializeField] Transform[] raycastPoints;

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

        if (IsOnGround())
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

    bool IsOnGround()
    {
        foreach (var point in raycastPoints)
        {
            if (Physics.Raycast(point.position, Vector3.down, raycastDepth, groundLayer))
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (raycastPoints == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (var point in raycastPoints)
            Gizmos.DrawLine(point.position, point.position + Vector3.down * raycastDepth);
    }

}
