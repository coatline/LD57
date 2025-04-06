using UnityEngine;

public interface IInteractable
{
    void Interact(Interactor interactor);
    bool CanInteract();
    string InteractText { get; }
}
