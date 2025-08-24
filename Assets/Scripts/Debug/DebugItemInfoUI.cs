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
                sb.Append($"���ݷ�: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKDAMAGE_PERCENT))
            {
                sb.Append($"���ݷ�: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_FLAT))
            {
                sb.Append($"���ݼӵ�: {itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_PERCENT))
            {
                sb.Append($"���ݼӵ�: +{itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_FLAT))
            {
                sb.Append($"�̵��ӵ�: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_PERCENT))
            {
                sb.Append($"�̵��ӵ�: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_FLAT))
            {
                sb.Append($"���ݹ���: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_PERCENT))
            {
                sb.Append($"���ݹ���: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALCHANCE_PERCENT))
            {
                sb.Append($"ġ��Ÿ Ȯ��: +{itemStatModifiers[GlobalDefine.STAT_CRITICALCHANCE_PERCENT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALDAMAGE_FLAT))
            {
                sb.Append($"ġ��Ÿ ����: +{itemStatModifiers[GlobalDefine.STAT_CRITICALDAMAGE_FLAT].value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_FLAT))
            {
                sb.Append($"ü��: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_FLAT].value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_PERCENT))
            {
                sb.Append($"ü��: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_PERCENT].value * 100}%\n");
            }

        }

        itemStatDescriptionText.text = sb.ToString();

        itemJobText.text = (itemData.itemID / 100000) switch
        {
            1 => "����",
            2 => "����",
            3 => "������",
            4 => "�ü�",
            _ => "�� �� ����"
        };

        itemEquipmentTypeText.text = itemData.itemEquipType.ToString();
        itemRarityText.text = itemData.itemRarity.ToString();
    }

}
