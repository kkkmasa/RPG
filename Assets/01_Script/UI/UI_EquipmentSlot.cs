using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }
}
