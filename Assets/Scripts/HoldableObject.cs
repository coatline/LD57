using UnityEngine;

public class HoldableObject : MonoBehaviour, IInteractable
{
    [SerializeField] SoundType pickupSound;

    bool hasBeenPickedUp;

    public virtual bool CanInteract(Interactor interactor) => interactor.ObjectHolder.HandEmpty;

    public virtual void Interact(Interactor interactor)
    {
        interactor.ObjectHolder.PickupItem(this);
        SoundPlayer.I.PlaySound(pickupSound, transform.position);

        if (hasBeenPickedUp == false)
        {
            hasBeenPickedUp = true;
            FirstTimeBeingPickedUp();
        }
    }

    protected virtual void FirstTimeBeingPickedUp() { }

    public string InteractText => "take (e)";
}
