using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] ObjectHolder objectHolder;

    public ObjectHolder ObjectHolder => objectHolder;


    protected IInteractable currentTarget;

    public void TryInteract()
    {
        if (currentTarget != null && currentTarget.CanInteract(this))
            currentTarget.Interact(this);

        currentTarget = null;
    }
}
