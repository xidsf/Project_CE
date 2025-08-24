using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    public Image BgImage;
    public Image ItemIconImage;

    private long serialNum;
    private int itemID;

    public void ResetItem()
    {
        BgImage.color = Color.white;
        ItemIconImage.sprite = null;
    }

    public void SetItem(long itemSerialNum)
    {
        serialNum = itemSerialNum;
        if (ItemIconImage.sprite != null)
        {
            Logger.LogError($"{itemSerialNum}: {gameObject.name} is already exist. Please Reset Slot First.");
            return;
        }

        var inventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        var item = inventoryData.InventoryItems.FirstOrDefault(i => i.serialNumber == itemSerialNum);
        if(item == null)
        {
            Logger.LogError($"Item with serial number {itemSerialNum} not found in inventory.");
            return;
        }
        itemID = item.itemID;

        var itemData = DataTableManager.Instance.GetItemData(itemID);
        Sprite itemIcon = null;
        if (ItemIconLoader.LoadItemIcon(itemID, out itemIcon))
        {
            ItemIconImage.sprite = itemIcon;
            BgImage.color = Item.GetRarityColor(itemData.itemRarity);
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
