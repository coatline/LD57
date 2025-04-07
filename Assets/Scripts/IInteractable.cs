using UnityEngine;

public interface IInteractable
{
    void Interact(Interactor interactor);
    bool CanInteract(Interactor interactor);
    string InteractText { get; }
}
