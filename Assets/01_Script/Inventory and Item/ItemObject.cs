using UnityEngine;

public class ItemObject : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] ItemData itemData;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()) {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
    void OnValidate()
    {
        if (!itemData) return;
        if (sr == null)
            this.sr = GetComponent<SpriteRenderer>();

        SetData(itemData);
    }

    void SetData(ItemData _itemData) {
        this.sr.sprite = _itemData.icon;
        this.gameObject.name = "item object - " + _itemData.itemName;
    }
}
