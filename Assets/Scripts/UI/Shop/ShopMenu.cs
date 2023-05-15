using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] int speedBoostAmount = 0;
    [SerializeField] int jumpBoostAmount = 0;

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
        Invoke(nameof (UpdateUI), .5f);
    }

    void UpdateUI()
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            priceAmountTexts[i].text = "" + powerUp[i].boostPrices;
        }

        DisplayBoostIndexText(speedBoostAmount, 0);
        DisplayBoostIndexText(jumpBoostAmount, 1);
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
    }

    public void LoadData(GameData data)
    {
        this.speedBoostAmount = data.speedBoostAmount;
        this.jumpBoostAmount = data.jumpBoostAmount;
    }

    void PurchaseBoost()
    {
        Inventory.instance.EmptyInventoryAndRefill(itemNames, itemAmount);
        Inventory.instance.GetItems();
    }

    public void SpeedBoost(PowerUp powerUp)
    {

        if (speedBoostAmount < powerUp.maxAmount && itemAmount[0] >= powerUp.boostPrices)
        {
            itemAmount[0] -= powerUp.boostPrices;

            //speedMult *= (speedBoostAmount + 1);

            PlayerController.instance.force *= speedMult;

            speedBoostAmount++;

            purchaseAmountTexts[0].text = "" + powerUp.boostName + " " + speedBoostAmount + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }

    public void JumpBoost(PowerUp powerUp)
    {
        if (jumpBoostAmount < powerUp.maxAmount && itemAmount[0] >= powerUp.boostPrices)
        {
            itemAmount[0] -= powerUp.boostPrices;

            //jumpMult *= (jumpBoostAmount + 1);

            PlayerController.instance.jumpForce *= jumpMult;

            jumpBoostAmount++;

            purchaseAmountTexts[1].text = "" + powerUp.boostName + " " + jumpBoostAmount + " / " + powerUp.maxAmount;
        }

        PurchaseBoost();
    }

    public void CloseShop()
    {
        Inventory.instance.EmptyInventoryAndRefill(itemNames, itemAmount);
        PlayerController.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        shopPanel.SetActive(false);
    }
}
