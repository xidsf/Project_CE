using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugItemInfoUIData : BaseUIData
{
    public int itemID;
}

public class DebugItemInfoUI : BaseUI
{
    public Image itemRarityBgImage;
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemRarityText;
    public TextMeshProUGUI itemJobText;
    public TextMeshProUGUI itemEquipmentTypeText;
    public TextMeshProUGUI itemStatDescriptionText;

    private DebugItemInfoUIData itemInfoData;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        itemInfoData = data as DebugItemInfoUIData;

        if (itemInfoData == null)
        {
            Logger.LogError("ItemInfoData is null");
            return;
        }

        var itemData = DataTableManager.Instance.GetItemData(itemInfoData.itemID);
        if (itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {itemInfoData.itemID}");
            return;
        }

        Sprite loadedSprite = null;

        if (ItemIconLoader.LoadItemIcon(itemData.itemID, out loadedSprite))
        {
            itemIconImage.sprite = loadedSprite;
            itemRarityBgImage.color = Item.GetRarityColor(itemData.itemRarity);
        }
        else
        {
            Logger.LogError($"Failed to load icon for item ID: {itemInfoData.itemID}");
            return;
        }

        itemNameText.text = itemData.itemName;
        itemRarityText.text = itemData.itemRarity.ToString();

        itemData = DataTableManager.Instance.GetItemData(itemInfoData.itemID);
        if (itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {itemInfoData.itemID}");
            return;
        }
        var itemStatModifiers = itemData.StatModifiers;

        StringBuilder sb = new StringBuilder();

        if (itemStatModifiers == null)
        {
            itemStatDescriptionText.text = "No stats available for this item.";
        }
        else
        {
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKDAMAGE_FLAT))
            {
                sb.Append($"공격력: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKDAMAGE_PERCENT))
            {
                sb.Append($"공격력: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_FLAT))
            {
                sb.Append($"공격속도: {itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_PERCENT))
            {
                sb.Append($"공격속도: +{itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_FLAT))
            {
                sb.Append($"이동속도: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_PERCENT))
            {
                sb.Append($"이동속도: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_FLAT))
            {
                sb.Append($"공격범위: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_PERCENT))
            {
                sb.Append($"공격범위: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALCHANCE_PERCENT))
            {
                sb.Append($"치명타 확률: +{itemStatModifiers[GlobalDefine.STAT_CRITICALCHANCE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALDAMAGE_FLAT))
            {
                sb.Append($"치명타 피해: +{itemStatModifiers[GlobalDefine.STAT_CRITICALDAMAGE_FLAT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_FLAT))
            {
                sb.Append($"체력: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_PERCENT))
            {
                sb.Append($"체력: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_PERCENT].value * 100}%\n");
            }

        }

        itemStatDescriptionText.text = sb.ToString();

        itemJobText.text = (itemData.itemID / 100000) switch
        {
            1 => "공용",
            2 => "전사",
            3 => "마법사",
            4 => "궁수",
            _ => "알 수 없음"
        };

        itemEquipmentTypeText.text = itemData.itemEquipType.ToString();
        itemRarityText.text = itemData.itemRarity.ToString();
    }

}
