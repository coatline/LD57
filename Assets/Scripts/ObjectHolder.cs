using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] Transform hand;
    GameObject currentItem;

    public void PickupItem(GameObject go)
    {
        if (go.GetComponent<Collider>() != null)
        {
            go.GetComponent<Collider>().enabled = false;
        }

        currentItem = go;
    }

    public void DropItem()
    {
        if (currentItem == null)
            return;

        if (currentItem.GetComponent<Collider>() != null)
        {
            currentItem.GetComponent<Collider>().enabled = true;
        }

        currentItem = null;
    }

    private void Update()
    {
        if (currentItem == null)
            return;

        currentItem.transform.position = hand.position;
        currentItem.transform.rotation = hand.rotation;
    }
}
