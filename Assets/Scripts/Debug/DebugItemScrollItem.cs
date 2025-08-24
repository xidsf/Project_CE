using Gpm.Ui;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugItemScrollData : InfiniteScrollData
{
    public int itemID;
}

public class DebugItemScrollItem : InfiniteScrollItem
{
    public Image itemImageBg;
    public Image itemImage;

    DebugItemScrollData infiniteScrollData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        infiniteScrollData = scrollData as DebugItemScrollData;

        if (infiniteScrollData == null)
        {
            Logger.Log("infiniteScrollData is invalid.");
            return;
        }

        ItemData itemData = DataTableManager.Instance.GetItemData(infiniteScrollData.itemID);
        if (itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {infiniteScrollData.itemID}");
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
            Logger.LogError($"Failed to load icon for item ID: {infiniteScrollData.itemID}");
            return;
        }
    }

    public void OnClickItem()
    {
        if (infiniteScrollData == null)
        {
            Logger.LogError("infiniteScrollData is null.");
            return;
        }

        var itemInfoData = new DebugItemInfoUIData
        {
            itemID = infiniteScrollData.itemID,
        };

        UIManager.Instance.OpenDebugUI<DebugItemInfoUI>(itemInfoData);
    }

}
