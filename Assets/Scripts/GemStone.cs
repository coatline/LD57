using UnityEngine;

public class GemStone : MonoBehaviour, IInteractable
{
    public bool CanInteract() => true;

    public void Interact(Interactor interactor)
    {
        interactor.ObjectHolder.PickupItem(gameObject);
        LevelManager.I.GrabbedGemStone();
    }

    public string InteractText => "take";
}
