using Gpm.Ui;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUIData : InfiniteScrollData
{
    public long SerialNumber;
    public int ItemId;
}

public class InventorySlotUIItem : InfiniteScrollItem
{
    public Image ItemImageBg;
    public Image ItemImage;

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

        if (ResourcesLoader.LoadItemIcon(itemData.ItemID, out sprite))
        {
            ItemImage.sprite = sprite;
            ItemImageBg.color = Item.GetRarityColor(itemData.ItemRarity);
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
        var itemData = userInventoryData.InventoryItems.Find(item => item.SerialNumber == infiniteScrollData.SerialNumber);
        if (itemData == null)
        {
            Logger.LogError($"Item with serial number {infiniteScrollData.SerialNumber} not found in inventory.");
            return;
        }
        var itemInfoData = new ItemInfoUIData
        {
            ItemID = infiniteScrollData.ItemId,
            IsEquipped = itemData.IsEquipped,
            SerialNumer = infiniteScrollData.SerialNumber
        };

        UIManager.Instance.OpenUI<ItemInfoUI>(itemInfoData);
    }

    
}
