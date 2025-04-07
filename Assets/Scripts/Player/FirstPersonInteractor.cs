using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstPersonInteractor : Interactor
{
    [SerializeField] Camera playerCamera;
    [SerializeField] TMP_Text interactTextUI;
    [SerializeField] float interactRange = 3f;

    IInteractable triggerInteractable;
    IInteractable raycastInteractable;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        raycastInteractable = null;

        if (Physics.Raycast(ray, out hit, interactRange))
            raycastInteractable = hit.collider.GetComponent<IInteractable>();

        if (raycastInteractable != null)
            Hover(raycastInteractable);
        else if (triggerInteractable != null)
            Hover(triggerInteractable);
        else
            interactTextUI.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            triggerInteractable = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerInteractable == null) return;

        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == triggerInteractable)
            triggerInteractable = null;
    }

    void Hover(IInteractable interactable)
    {
        if (interactable.CanInteract(this))
        {
            interactTextUI.text = interactable.InteractText;
            interactTextUI.enabled = true;

            currentTarget = interactable;
        }
        else
            interactTextUI.enabled = false;
    }
}
