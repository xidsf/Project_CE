using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] GameObject weaponEmptyIcon;
    [SerializeField] GameObject EquippedItemObject;
    [SerializeField] Image EquippedItemBgImage;
    [SerializeField] Image EquippedItemIconImage;

    private long serialNum;
    private int itemID;

    public void ResetItem()
    {
        EquippedItemBgImage.color = Color.white;
        EquippedItemIconImage.sprite = null;
        weaponEmptyIcon.SetActive(true);
        EquippedItemObject.SetActive(false);
    }

    public void SetItem(long itemSerialNum)
    {
        EquippedItemObject.SetActive(true);
        weaponEmptyIcon.SetActive(false);
        serialNum = itemSerialNum;
        if (EquippedItemIconImage.sprite != null)
        {
            Logger.LogError($"{itemSerialNum}: {gameObject.name} is already exist. Please Reset Slot First.");
            return;
        }

        var inventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        var item = inventoryData.InventoryItems.Find(i => i.serialNumber == itemSerialNum);
        if (item == null)
        {
            Logger.LogError($"Item with serial number {itemSerialNum} not found in inventory.");
            return;
        }
        itemID = item.itemID;

        var itemData = DataTableManager.Instance.GetItemData(itemID);
        Sprite itemIcon = null;
        if (ItemIconLoader.LoadItemIcon(itemID, out itemIcon))
        {
            EquippedItemIconImage.sprite = itemIcon;
            EquippedItemBgImage.color = Item.GetRarityColor(itemData.itemRarity);
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
            itemID = itemID,
            isEquipped = true,
            serialNumer = serialNum
        };

        UIManager.Instance.OpenUI<ItemInfoUI>(data);
    }
}
