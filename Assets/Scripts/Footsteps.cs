using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] float footstepDistance;
    [SerializeField] SoundType footstepSound;

    Vector3 prevPosition;

    private void Awake()
    {
        prevPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(prevPosition, transform.position) > footstepDistance)
        {
            prevPosition = transform.position;
            SoundPlayer.I.PlaySound(footstepSound, transform.position);
        }
    }
}
