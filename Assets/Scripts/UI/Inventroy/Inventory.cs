using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
public class Inventory : MonoBehaviour, IDataPersistence
{
    [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

    [Space]
    [Header("Inventory Menu Components")]
    public GameObject inventoryMenu;
    public GameObject itemPanel;
    public GameObject itemPanelGrid;
    public GameObject pp;
    public GameObject otherUI;

    [SerializeField] GameObject pauseMenu;

    public TextMeshProUGUI[] itemAmountText;


    public static Inventory instance;

    public Mouse mouse;

    private List<ItemPanel> existingPanels = new List<ItemPanel>();

    Dictionary<string, Item> allItemsDictionary = new Dictionary<string, Item>();

    [Space]
    public int inventorySize = 24;

    List<int> rockIndex = new List<int>();
    List<int> crystalIndex = new List<int>();
    List<int> goldIndex = new List<int>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlotInfo(null, 0));
        }

        List<Item> allItems = GetAllItems().ToList();
        string itemsInDictionary = "Items in Dictionary: ";
        foreach (Item i in allItems)
        {
            if (!allItemsDictionary.ContainsKey(i.GiveName()))
            {
                allItemsDictionary.Add(i.GiveName(), i);
                itemsInDictionary += ", " + i.GiveName();
            }
            else
            {
                Debug.Log("" + i + " already exists in Dictionary - shares name with " + allItemsDictionary[i.GiveName()]);
            }
        }
        itemsInDictionary += ".";
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GetItems), .5f);
    }

    public void LoadData(GameData data)
    {
        if (data.rock > 0) AddItem("Rock", data.rock);
        if (data.crystal > 0) AddItem("Crystal", data.crystal);
        if (data.gold > 0) AddItem("Gold", data.gold);
    }

    public void SaveData(GameData data)
    {
        data.rock = GetItemAmount("Rock", 0);
        data.crystal = GetItemAmount("Crystal", 1);
        data.gold = GetItemAmount("Gold", 2);
    }

    public void GetItems()
    {
        GetItemAmount("Rock", 0);
        GetItemAmount("Crystal", 1);
        GetItemAmount("Gold", 2);
    }

    public int GetItemAmount(string itemName, int i)
    {
        RefreshInventory();

        return SetItemAmount(existingPanels, itemName, i); ;
    }

    int SetItemAmount(List<ItemPanel> ips, string item, int i)
    {
        List<int> itemIndex = new List<int>();

        foreach (ItemPanel ip in ips)
        {
            if (ip.itemImage.sprite != null)
            {
                if (ip.itemImage.sprite.name == item)
                {
                    itemIndex.Add(ip.itemSlot.stacks);
                }
            }
        }

        int finalValue = 0;

        foreach (int y in itemIndex)
        {
            finalValue += y;
        }

        itemAmountText[i].text = item + " " + finalValue.ToString();

        return finalValue;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
#endif
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryMenu.activeSelf)
            {
                inventoryMenu.SetActive(false);
                mouse.EmptySlot();
                Cursor.lockState = CursorLockMode.Locked;
                PlayerController.canMove = true;
                pp.SetActive(false);
                UpdateOtherUI(true);
            }
            else if (!pauseMenu.activeSelf)
            {
                inventoryMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                RefreshInventory();
                PlayerController.canMove = false;
                pp.SetActive(true);
                UpdateOtherUI(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && mouse.itemSlot.item != null)
        {
            RefreshInventory();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && mouse.itemSlot.item != null && !EventSystem.current.IsPointerOverGameObject())
        {
            DropItem(mouse.itemSlot.item.GiveName());
        }
    }

    void UpdateOtherUI(bool b)
    {
        otherUI.SetActive(b);
    }

    public void EmptyInventoryAndRefill(string[] itemName, int[] itemAmount)
    {
        foreach(ItemPanel ip in existingPanels)
        {
            ClearSlot(ip.itemSlot);
        }

        for (int y = 0; y < itemName.Length; y++)
        {
            AddItem(itemName[y], itemAmount[y]);
        }

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        existingPanels = itemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

        //Create Panels if needed
        if (existingPanels.Count < inventorySize)
        {
            int amountToCreate = inventorySize - existingPanels.Count;
            for (int i = 0; i < amountToCreate; i++)
            {
                GameObject newPanel = Instantiate(itemPanel, itemPanelGrid.transform);
                existingPanels.Add(newPanel.GetComponent<ItemPanel>());
            }
        }

        int index = 0;
        foreach (ItemSlotInfo i in items)
        {
            //Name our List Elements
            i.name = "" + (index + 1);
            if (i.item != null) i.name += ": " + i.item.GiveName();
            else i.name += ": -";

            //Update our Panels
            ItemPanel panel = existingPanels[index];
            panel.name = i.name + " Panel";
            if (panel != null)
            {
                panel.inventory = this;
                panel.itemSlot = i;
                if (i.item != null)
                {
                    panel.itemImage.gameObject.SetActive(true);
                    panel.itemImage.sprite = i.item.GiveItemImage();
                    panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                    panel.stacksText.gameObject.SetActive(true);
                    panel.stacksText.text = "" + i.stacks;
                }
                else
                {
                    panel.itemImage.gameObject.SetActive(false);
                    panel.stacksText.gameObject.SetActive(false);
                }
            }
            index++;
        }
        mouse.EmptySlot();
    }

    public int AddItem(string itemName, int amount)
    {
        //Find Item to add
        Item item = null;
        allItemsDictionary.TryGetValue(itemName, out item);
        //Exit method if no Item was found
        if (item == null)
        {
            Debug.Log("Could not find Item in Dictionary to add to Inventory");
            return amount;
        }

        //Check for open spaces in existing slots
        foreach(ItemSlotInfo i in items)
        {
            if (i.item != null)
            {
                if (i.item.GiveName() == item.GiveName())
                {
                    if (amount > i.item.MaxStacks() - i.stacks)
                    {
                        amount -= i.item.MaxStacks() - i.stacks;
                        i.stacks = i.item.MaxStacks();
                    }
                    else
                    {
                        i.stacks += amount;
                        if (inventoryMenu.activeSelf) RefreshInventory();
                        return 0;
                    }
                }
            }
        }
        //Fill empty slots with leftover items
        foreach(ItemSlotInfo i in items)
        {
            if (i.item == null)
            {
                if (amount > item.MaxStacks())
                {
                    i.item = item;
                    i.stacks = item.MaxStacks();
                    amount -= item.MaxStacks();
                }
                else
                {
                    i.item = item;
                    i.stacks = amount;
                    if (inventoryMenu.activeSelf) RefreshInventory();
                    return 0;
                }
            }
        }
        //No space in Inventory, return remainder items
        Debug.Log("No space in Inventory for: " + item.GiveName());
        if (inventoryMenu.activeSelf) RefreshInventory();
        return amount;
    }

    public void DropItem(string itemName)
    {
        //Find Item to add
        Item item = null;
        allItemsDictionary.TryGetValue(itemName, out item);
        //Exit method if no Item was found
        if (item == null)
        {
            Debug.Log("Could not find Item in Dictionary to add to drop");
            return;
        }

        item.GiveItemMesh();

        Transform camTransform = Camera.main.transform;

        GameObject droppedItem = Instantiate(item.DropObject(),
            camTransform.position + (camTransform.forward * 2),
            Quaternion.Euler(Vector3.zero));

        droppedItem.GetComponentInChildren<MeshRenderer>().material = item.GiveItemMat();
        droppedItem.GetComponentInChildren<MeshFilter>().mesh = item.GiveItemMesh();
        droppedItem.transform.GetChild(1).localScale = new Vector3(item.GiveScale(), item.GiveScale(), item.GiveScale());

        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = camTransform.forward * 12;

        ItemPickup ip = droppedItem.GetComponentInChildren<ItemPickup>();
        if (ip != null)
        {
            ip.itemToDrop = itemName;
            ip.amount = mouse.splitSize;
            mouse.itemSlot.stacks -= mouse.splitSize;
        }

        if (mouse.itemSlot.stacks < 1) ClearSlot(mouse.itemSlot);
        mouse.EmptySlot();
        RefreshInventory();

        GetItems();
    }

    public void ClearSlot(ItemSlotInfo slot)
    {
        slot.item = null;
        slot.stacks = 0;
    }

    IEnumerable<Item> GetAllItems()
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(Item)))
            .Select(type => System.Activator.CreateInstance(type) as Item);
    }
}
