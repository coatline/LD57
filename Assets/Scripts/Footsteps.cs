using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] float footstepDistance;
    [SerializeField] SoundType footstepSound;
    [SerializeField] GroundDetector groundDetector;

    Vector3 prevPosition;
    float distanceTraveled;

    private void Awake()
    {
        prevPosition = transform.position;
    }

    private void Start()
    {
        if (groundDetector != null)
            groundDetector.JustGrounded += GroundDetector_JustGrounded;
    }

    private void GroundDetector_JustGrounded()
    {
        distanceTraveled = footstepDistance;
    }

    void Update()
    {
        if (groundDetector == null || groundDetector.IsOnGround)
        {
            distanceTraveled += Vector3.Distance(new Vector3(prevPosition.x, 0, prevPosition.z), new Vector3(transform.position.x, 0, transform.position.z));

            if (distanceTraveled >= footstepDistance)
            {
                distanceTraveled = 0;
                SoundPlayer.I.PlaySound(footstepSound, transform.position);
            }
        }

        prevPosition = transform.position;
    }
}
