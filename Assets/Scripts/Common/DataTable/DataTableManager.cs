using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        foreach (var item in readData)
        {
            var itemData = new ItemData
            {
                itemID = Convert.ToInt32(item["item_id"]),
                itemName = item["item_name"].ToString(),
                moveSpeed = Convert.ToSingle(item["move_speed"]),
                attackRange = Convert.ToSingle(item["attack_range"]),
                attackDamage = Convert.ToSingle(item["attack_damage"]),
                attackSpeed = Convert.ToSingle(item["attack_speed"]),
                criticalChange = Convert.ToSingle(item["crit_chance"]),
                criticalDamage = Convert.ToSingle(item["crit_damage"])
            };

            itemDataList.Add(itemData);
        }
    }
    
    public ItemData GetItemData(int itemID)
    {
        return itemDataList.Where(item => item.itemID == itemID).FirstOrDefault();
    }

    #endregion

}
