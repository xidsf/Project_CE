using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUIData : BaseUIData
{
    public int ItemID;
    public bool IsEquipped;
    public long SerialNumer;
}

public class ItemInfoUI : BaseUI
{
    [SerializeField] private Image itemRarityBgImage;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemRarityText;
    [SerializeField] private TextMeshProUGUI itemEquipmentTypeText;
    [SerializeField] private TextMeshProUGUI itemStatDescriptionText;
    [SerializeField] private TextMeshProUGUI itemEquipButtonText;

    private ItemInfoUIData itemInfoData;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        itemInfoData = data as ItemInfoUIData;

        if(itemInfoData == null)
        {
            Logger.LogError("ItemInfoData is null");
            return;
        }

        var itemData = DataTableManager.Instance.GetItemData(itemInfoData.ItemID);
        if(itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {itemInfoData.ItemID}");
            return;
        }

        Sprite loadedSprite = null;

        if(ResourcesLoader.LoadItemIcon(itemData.ItemID, out loadedSprite))
        {
            itemIconImage.sprite = loadedSprite;
            itemRarityBgImage.color = Item.GetRarityColor(itemData.ItemRarity);
        }
        else
        {
            Logger.LogError($"Failed to load icon for item ID: {itemInfoData.ItemID}");
            return;
        }

        itemNameText.text = itemData.ItemName;
        itemRarityText.text = itemData.ItemRarity.ToString();
        
        if (itemData == null)
        {
            Logger.LogError($"ItemData not found for item ID: {itemInfoData.ItemID}");
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
            if(itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKDAMAGE_FLAT))
            {
                sb.Append($"공격력: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_FLAT].Value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKDAMAGE_PERCENT))
            {
                sb.Append($"공격력: +{itemStatModifiers[GlobalDefine.STAT_ATTACKDAMAGE_PERCENT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_FLAT))
            {
                sb.Append($"공격속도: {itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_FLAT].Value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKSPEED_PERCENT))
            {
                sb.Append($"공격속도: +{itemStatModifiers[GlobalDefine.STAT_ATTACKSPEED_PERCENT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_FLAT))
            {
                sb.Append($"이동속도: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_FLAT].Value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_MOVESPEED_PERCENT))
            {
                sb.Append($"이동속도: +{itemStatModifiers[GlobalDefine.STAT_MOVESPEED_PERCENT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_FLAT))
            {
                sb.Append($"공격범위: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_FLAT].Value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_ATTACKRANGE_PERCENT))
            {
                sb.Append($"공격범위: +{itemStatModifiers[GlobalDefine.STAT_ATTACKRANGE_PERCENT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALCHANCE_PERCENT))
            {
                sb.Append($"치명타 확률: +{itemStatModifiers[GlobalDefine.STAT_CRITICALCHANCE_PERCENT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_CRITICALDAMAGE_FLAT))
            {
                sb.Append($"치명타 피해: +{itemStatModifiers[GlobalDefine.STAT_CRITICALDAMAGE_FLAT].Value * 100}%\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_FLAT))
            {
                sb.Append($"체력: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_FLAT].Value}\n");
            }
            if (itemStatModifiers.ContainsKey(GlobalDefine.STAT_HEALTHPOINT_PERCENT))
            {
                sb.Append($"체력: +{itemStatModifiers[GlobalDefine.STAT_HEALTHPOINT_PERCENT].Value * 100}%\n");
            }

        }

        itemStatDescriptionText.text = sb.ToString();

        itemRarityText.text = itemData.ItemRarity.ToString();
        itemRarityText.color = Item.GetRarityColor(itemData.ItemRarity);
        itemEquipmentTypeText.text = itemData.ItemEquipType.ToString();

        if (itemInfoData.IsEquipped)
        {
            itemEquipButtonText.text = "해제";
        }
        else
        {
            itemEquipButtonText.text = "장착";
        }
    }

    public void OnClickEquipButton()
    {
        if (itemInfoData == null)
        {
            Logger.LogError("ItemInfoData does not set");
            return;
        }

        var userData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if (userData == null)
        {
            Logger.LogError("UserInventoryData is null");
            return;
        }

        var userEquipType = (ItemEquipType)(itemInfoData.ItemID / 1000 % 10);

        if (itemInfoData.IsEquipped)
        {
            userData.UnequipItem(itemInfoData.SerialNumer);
        }
        else
        {
            userData.EquipItem(itemInfoData.SerialNumer);
        }
        userData.SaveData();

        InventoryUI inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        if (inventoryUI == null)
        {
            Logger.LogError($"InventoryUI is not null. inventoryUI Refresh Failed");
            return;
        }
        inventoryUI.RefreshInventoryUI();
        CloseUI();
    }
}
