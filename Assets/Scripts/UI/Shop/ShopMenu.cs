using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour, IDataPersistence
{
    [Header("UI Display")]
    [Space]
    [SerializeField] TextMeshProUGUI[] purchaseAmountTexts;
    [SerializeField] TextMeshProUGUI[] priceAmountTexts;

    [Space]
    [SerializeField] PowerUp[] powerUp;

    [Space]
    [SerializeField] GameObject[] objects;
    [SerializeField] Button[] otherButtons;
    [SerializeField] GameObject[] shops;

    [Space]
    public int speedBoostAmount = 0;
    public int jumpBoostAmount = 0;
    public int minimap = 0;
    public int flashlight = 0;

    [Header("Item Properties")]
    [Space]
    [SerializeField] int[] itemAmount = { 0, 0, 0 };
    [SerializeField] string[] itemNames = { "Rock", "Crystal", "Gold" };

    [Space]
    public float speedMult = 0;
    public float jumpMult = 0;

    [Space]
    public GameObject shopPanel;

    public static ShopMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke(nameof (UpdateScene), .5f);
    }

    void UpdateScene()
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            priceAmountTexts[i].text = "" + powerUp[i].boostPrices;
        }

        DisplayBoostIndexText(speedBoostAmount, 0);
        DisplayBoostIndexText(jumpBoostAmount, 1);
        DisplayBoostIndexText(minimap, 2);
        DisplayBoostIndexText(flashlight, 3);

        UpdateObjects(speedBoostAmount, objects[0]);
        UpdateObjects(jumpBoostAmount, objects[1]);
        UpdateObjects(minimap, objects[2]);
        UpdateObjects(flashlight, objects[3]);
    }

    void UpdateObjects(int i, GameObject o)
    {
        if (i > 0)
        {
            o.SetActive(true);
        }
        else
        {
            o.SetActive(false);
        }
    }

    void DisplayBoostIndexText(int amount, int i)
    {
        purchaseAmountTexts[i].text = "" + powerUp[i].boostName + " " + amount + " / " + powerUp[i].maxAmount;
    }

    public void GetItemAmounts()
    {
        for (int i = 0; i < itemNames.Length; i++)
        {
            itemAmount[i] = Inventory.instance.GetItemAmount(itemNames[i], i);
        }
    }

    public void SaveData(GameData data)
    {
        data.speedBoostAmount = this.speedBoostAmount;
        data.jumpBoostAmount = this.jumpBoostAmount;
        data.speedMult = this.speedMult;
        data.jumpMult = this.jumpMult;
        data.minimap = this.minimap;
        data.flashlight = this.flashlight;
    }

    public void LoadData(GameData data)
    {
        this.speedBoostAmount = data.speedBoostAmount;
        this.jumpBoostAmount = data.jumpBoostAmount;
        this.speedMult = data.speedMult;
        this.jumpMult = data.jumpMult;
        this.minimap = data.minimap;
        this.flashlight = data.flashlight;
    }

    void PurchaseBoost()
    {
        Inventory.instance.EmptyInventoryAndRefill(itemNames, itemAmount);
        Inventory.instance.GetItems();
    }

    public void SpeedBoost(PowerUp powerUp)
    {

        if (speedBoostAmount < powerUp.maxAmount && itemAmount[2] >= powerUp.boostPrices)
        {
            itemAmount[2] -= powerUp.boostPrices;

            objects[0].SetActive(true);

            speedBoostAmount++;

            purchaseAmountTexts[0].text = "" + powerUp.boostName + " " + speedBoostAmount + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }

    public void JumpBoost(PowerUp powerUp)
    {
        if (jumpBoostAmount < powerUp.maxAmount && itemAmount[2] >= powerUp.boostPrices)
        {
            itemAmount[2] -= powerUp.boostPrices;

            objects[1].SetActive(true);

            jumpBoostAmount++;

            purchaseAmountTexts[1].text = "" + powerUp.boostName + " " + jumpBoostAmount + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }

    public void Minimap(PowerUp powerUp)
    {
        if (minimap < powerUp.maxAmount && itemAmount[1] >= powerUp.boostPrices)
        {
            itemAmount[1] -= powerUp.boostPrices;

            objects[2].SetActive(true);

            minimap++;

            purchaseAmountTexts[2].text = "" + powerUp.boostName + " " + minimap + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }
    public void Flashlight(PowerUp powerUp)
    {
        if (flashlight < powerUp.maxAmount && itemAmount[1] >= powerUp.boostPrices)
        {
            itemAmount[1] -= powerUp.boostPrices;

            objects[3].SetActive(true);

            flashlight++;

            purchaseAmountTexts[3].text = "" + powerUp.boostName + " " + flashlight + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }

    public void ChangeShop(int i)
    {
        foreach (GameObject g in shops)
        {
            g.SetActive(false);
        }

        shops[i].SetActive(true);

        foreach (Button b in otherButtons)
        {
            if (b != otherButtons[i])
            {
                b.interactable = true;
            }
            else
            {
                b.interactable = false;
            }
        }
    }

    public void CloseShop()
    {
        Inventory.instance.EmptyInventoryAndRefill(itemNames, itemAmount);
        PlayerController.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        shopPanel.SetActive(false);
    }
}
