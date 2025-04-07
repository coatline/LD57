using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] Transform hand;
    HoldableObject currentObject;

    public void PickupItem(HoldableObject holdableObject)
    {
        if (holdableObject.GetComponent<Rigidbody>() != null)
            holdableObject.GetComponent<Rigidbody>().isKinematic = true;

        currentObject = holdableObject;
    }

    public void TryDropItem()
    {
        if (currentObject == null)
            return;

        if (currentObject.GetComponent<Rigidbody>() != null)
            currentObject.GetComponent<Rigidbody>().isKinematic = false;

        currentObject = null;
    }

    private void Update()
    {
        if (currentObject == null)
            return;

        currentObject.transform.SetPositionAndRotation(hand.position, hand.rotation);
    }

    public HoldableObject CurrentItem => currentObject;
    public bool HandEmpty => currentObject == null;
}
