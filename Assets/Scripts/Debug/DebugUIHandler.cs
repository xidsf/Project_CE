using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIHandler : MonoBehaviour
{
    public InfiniteScroll ItemListScroll;

    public List<ItemData> items = new();

    private void Start()
    {
        var itemDatas = DataTableManager.Instance.GetAllItemDatas();

        foreach(var itemData in itemDatas)
        {
            DebugItemScrollData itemInfo = new DebugItemScrollData
            {
                itemID = itemData.itemID
            };
            ItemListScroll.InsertData(itemInfo);
        }

        

    }
}
