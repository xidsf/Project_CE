using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

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
            var itemData = new ItemData
            {
                itemID = Convert.ToInt32(item["item_id"]),
                itemName = item["item_name"].ToString()
            };

            foreach(var str in GlobalDefine.StatStrings)
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

                    if (statValue != 0)
                    {
                        itemData.StatModifiers.Add(str, new StatModifier(statValue, modifierType, this));
                    }
                }
            }

            itemData.itemRarity = (ItemRarity)(itemData.itemID / 10000 % 10);
            itemData.itemEquipType = (ItemEquipType)(itemData.itemID / 1000 % 10);

            itemDataList.Add(itemData);
        }
    }
    
    public ItemData GetItemData(int itemID)
    {
        return itemDataList.Where(item => item.itemID == itemID).FirstOrDefault();
    }

    public List<ItemData> GetAllItemDatas()
    {
        return itemDataList;
    }

    #endregion

}
