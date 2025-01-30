using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlotUI(InventoryItem _item)
    {
        this.item = _item;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot() {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear; 

        itemText.text = "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipmentItem(item.data);
        }

    }
}
