using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemToDrop;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Inventory.instance != null) PickUpItem(Inventory.instance);
        }
    }

    public void PickUpItem(Inventory inventory)
    {
        amount = inventory.AddItem(itemToDrop, amount);

        if (amount < 1) Destroy(this.gameObject.transform.parent.gameObject);

        inventory.GetItems();
    }

}
