using UnityEngine;

public class Wellivator : MonoBehaviour, IInteractable
{
    [SerializeField] float animationTime;
    [SerializeField] Transform bucket;
    [SerializeField] SoundType decendSound;

    float bucketTargetY;


    private void Awake()
    {
        bucketTargetY = 0;
    }

    private void FixedUpdate()
    {
        bucket.transform.position = new Vector3(bucket.transform.position.x, Mathf.MoveTowards(bucket.transform.position.y, bucketTargetY, Time.fixedDeltaTime), bucket.transform.position.z);
    }

    public Vector3 GetBucketPosition() => bucket.position;

    public string InteractText => "decend (e)";

    public bool CanInteract(Interactor interactor)
    {
        return interactor.ObjectHolder.CurrentItem != null && interactor.ObjectHolder.CurrentItem.name.Contains("Gemstone");
    }

    public void Interact(Interactor interactor)
    {
        SoundPlayer.I.PlaySound(decendSound, transform.position);
        bucketTargetY = -10;
        LevelManager.I.CompleteLevel();
    }
}