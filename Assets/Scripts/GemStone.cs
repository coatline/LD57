using UnityEngine;

public class GemStone : MonoBehaviour, IInteractable
{
    public bool CanInteract() => true;

    public void Interact()
    {
        LevelManager.I.GrabbedGemStone();
        Destroy(gameObject);
    }
}
