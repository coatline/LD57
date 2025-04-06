using TMPro;
using UnityEngine;

public class FirstPersonInteractor : Interactor
{
    [SerializeField] Camera playerCamera;
    [SerializeField] TMP_Text interactTextUI;
    [SerializeField] float interactRange = 3f;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        currentTarget = null;
        interactTextUI.enabled = false;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            currentTarget = hit.collider.GetComponent<IInteractable>();

            if (currentTarget != null)
            {
                if (currentTarget.CanInteract())
                {
                    interactTextUI.text = currentTarget.InteractText;
                    interactTextUI.enabled = true;
                }
            }
        }
    }
}
