using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] GameObject weaponEmptyIcon;
    [SerializeField] GameObject EquippedItemObject;
    [SerializeField] Image equippedItemBgImage;
    [SerializeField] Image equippedItemIconImage;

    private long serialNum;
    private int itemID;

    public void ResetItem()
    {
        equippedItemBgImage.color = Color.white;
        equippedItemIconImage.sprite = null;
        weaponEmptyIcon.SetActive(true);
        EquippedItemObject.SetActive(false);
    }

    public void SetItem(long itemSerialNum)
    {
        EquippedItemObject.SetActive(true);
        weaponEmptyIcon.SetActive(false);
        serialNum = itemSerialNum;
        if (equippedItemIconImage.sprite != null)
        {
            Logger.LogError($"{itemSerialNum}: {gameObject.name} is already exist. Please Reset Slot First.");
            return;
        }

        var inventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        var item = inventoryData.InventoryItems.Find(i => i.SerialNumber == itemSerialNum);
        if (item == null)
        {
            Logger.LogError($"Item with serial number {itemSerialNum} not found in inventory.");
            return;
        }
        itemID = item.ItemID;

        var itemData = DataTableManager.Instance.GetItemData(itemID);
        Sprite itemIcon = null;
        if (ItemIconLoader.LoadItemIcon(itemID, out itemIcon))
        {
            equippedItemIconImage.sprite = itemIcon;
            equippedItemBgImage.color = Item.GetRarityColor(itemData.ItemRarity);
        }
        else
        {
            Logger.LogError($"Failed to load icon for item ID: {itemID}");
        }
    }

    public void OnClickItem()
    {
        var data = new ItemInfoUIData
        {
            ItemID = itemID,
            IsEquipped = true,
            SerialNumer = serialNum
        };

        UIManager.Instance.OpenUI<ItemInfoUI>(data);
    }
}
