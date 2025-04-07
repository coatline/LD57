using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public event System.Action JustGrounded;

    [SerializeField] LayerMask groundLayer;

    [Header("Raycasts")]
    [SerializeField] float raycastDepth;
    [SerializeField] Transform[] raycastPoints;


    private void FixedUpdate()
    {
        foreach (var point in raycastPoints)
        {
            if (Physics.Raycast(point.position, Vector3.down, raycastDepth, groundLayer))
            {
                if (IsOnGround == false)
                {
                    JustGrounded?.Invoke();
                    IsOnGround = true;
                }

                return;
            }
        }

        IsOnGround = false;
    }


    private void OnDrawGizmos()
    {
        if (raycastPoints == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (var point in raycastPoints)
            Gizmos.DrawLine(point.position, point.position + Vector3.down * raycastDepth);
    }

    public bool IsOnGround { get; private set; }
}
