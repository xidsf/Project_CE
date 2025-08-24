using Gpm.Ui;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUIData : InfiniteScrollData
{
    public long serialNumber;
    public int ItemId;
}

public class InventorySlotUIItem : InfiniteScrollItem
{
    public Image itemImageBg;
    public Image itemImage;

    InventorySlotUIData infiniteScrollData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        infiniteScrollData = scrollData as InventorySlotUIData;

        if (infiniteScrollData == null)
        {
            Logger.Log("infiniteScrollData is invalid.");
            return;
        }

        ItemData itemData = DataTableManager.Instance.GetItemData(infiniteScrollData.ItemId);
        if(itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {infiniteScrollData.ItemId}");
            return;
        }

        Sprite sprite = null;

        if (ItemIconLoader.LoadItemIcon(itemData.itemID, out sprite))
        {
            itemImage.sprite = sprite;
            itemImageBg.color = Item.GetRarityColor(itemData.itemRarity);
        }
        else
        {
            Logger.LogError($"Failed to load icon for item ID: {infiniteScrollData.ItemId}");
            return;
        }
    }

    public void OnClickItem()
    {
        if(infiniteScrollData == null)
        {
            Logger.LogError("infiniteScrollData is null.");
            return;
        }

        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if (userInventoryData == null)
        {
            Logger.LogError("UserInventoryData is null.");
            return;
        }
        var itemData = userInventoryData.InventoryItems.FirstOrDefault(item => item.serialNumber == infiniteScrollData.serialNumber);
        if (itemData == null)
        {
            Logger.LogError($"Item with serial number {infiniteScrollData.serialNumber} not found in inventory.");
            return;
        }
        var itemInfoData = new ItemInfoUIData
        {
            itemID = infiniteScrollData.ItemId,
            isEquipped = itemData.isEquipped,
            serialNumer = infiniteScrollData.serialNumber
        };

        UIManager.Instance.OpenUI<ItemInfoUI>(itemInfoData);
    }

    
}
