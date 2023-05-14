using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : MonoBehaviour
{
    private GameObject shopPanel;

    private void Start()
    {
        shopPanel = ShopMenu.instance.shopPanel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.canMove = false;
            Cursor.lockState = CursorLockMode.Confined;
            ShopMenu.instance.GetItemAmounts();
            shopPanel.SetActive(true);
        }
    }
}
