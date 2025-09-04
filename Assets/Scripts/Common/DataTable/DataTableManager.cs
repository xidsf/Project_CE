using System;
using System.Collections.Generic;
using System.Text;

public class ChaaracterData
{
    public readonly int CharacterID;
    public readonly string CharacterName;
    public readonly float MoveSpeed;
    public readonly float AttackDamage;
    public readonly float AttackRange;
    public readonly float AttackSpeed;
    public readonly float CritChance;
    public readonly float CritDamage;
    public readonly float HP;
}

public class DataTableManager : Singleton<DataTableManager>
{
    private const string DATA_TABLE_PATH = "DataTables";
    
    protected override void Init()
    {
        base.Init();

        LoadItemDataTable();
    }

    #region ITEM_DATA

    
    private List<ItemData> itemDataList = new();
    private void LoadItemDataTable()
    {
        var readData = CSVReader.Read($"{DATA_TABLE_PATH}/ItemDataTable");
        StringBuilder sb = new();
        
        foreach (var item in readData)
        {
            Dictionary<string, ItemStatData> statModifiers = new();
            foreach (var str in GlobalDefine.StatModifierStrings)
            {
                var statValue = Convert.ToSingle(item[str]);
                if (statValue != 0)
                {
                    sb.Clear();
                    sb.Append(str);
                    ModifierType modifierType = ModifierType.Percent;

                    if (sb[4] == '_')
                    {
                        modifierType = ModifierType.Flat;
                    }
                    statModifiers.Add(str, new ItemStatData(statValue, modifierType));
                }
            }

            var itemID = Convert.ToInt32(item["item_id"]);

            var itemData = new ItemData(itemID, item["item_name"].ToString(),
                (ItemEquipType)(itemID / 1000 % 10), (ItemRarity)(itemID / 10000 % 10), statModifiers);

            itemDataList.Add(itemData);
        }
    }
    
    public ItemData GetItemData(int itemID)
    {
        return itemDataList.Find(item => item.ItemID == itemID);
    }

    public List<ItemData> GetAllItemDatas()
    {
        return itemDataList;
    }

    #endregion

    #region CHARACTER_DATA

    private List<ChaaracterData> characterDataList = new();

    #endregion
}
