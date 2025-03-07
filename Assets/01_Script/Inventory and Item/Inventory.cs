using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlopParent;
    [SerializeField] Transform equipmentSlotParent;
    UI_ItemSlot[] inventoryItemSlot;
    UI_ItemSlot[] stashItemSlot;
    UI_EquipmentSlot[] equipmentSlot;

    void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        this.stash = new List<InventoryItem>();
        this.stashDictionary = new Dictionary<ItemData, InventoryItem>();

        this.equipment = new List<InventoryItem>();
        this.equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();


        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        this.stashItemSlot = stashSlopParent.GetComponentsInChildren<UI_ItemSlot>();
        this.equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }
    void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++) {
            foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) {
                if (item.Key.equipmentType == equipmentSlot[i].slotType) {
                    equipmentSlot[i].UpdateSlotUI(item.Value);
                }
            }
        }
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlotUI(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlotUI(stash[i]);
        }
    }

    public void EquipmentItem(ItemData _itemData)
    {
        ItemData_Equipment newItem_equipment = _itemData as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newItem_equipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newItem_equipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        if (oldEquipment != null)
        {
            Unequipment(oldEquipment);
            AddItem(oldEquipment);
        }


        equipment.Add(newItem);
        equipmentDictionary.Add(newItem_equipment, newItem);
        newItem_equipment.AddModifiers();

        RemoveItem(_itemData);

        UpdateSlotUI();
    }

    private void Unequipment(ItemData_Equipment itemToDelete)
    {
        if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToDelete);
            itemToDelete.RemoveModifiers();
        }
    }

    public void AddItem(ItemData _itemData)
    {
        if (_itemData.itemType == ItemType.Equipment)
        {
            AddToInventory(_itemData);
        }
        else if (_itemData.itemType == ItemType.Material)
        {
            AddToStash(_itemData);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _itemData)
    {
        if (stashDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_itemData);
            stash.Add(newItem);
            stashDictionary.Add(_itemData, newItem);
        }
    }

    private void AddToInventory(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_itemData);
            inventory.Add(newItem);
            inventoryDictionary.Add(_itemData, newItem);
        }
    }

    public void RemoveItem(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_itemData);
            }
            else
            {
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_itemData, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_itemData);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }


        UpdateSlotUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ItemData itemData = inventory[inventory.Count - 1].data;
            RemoveItem(itemData);
        }

    }
}
