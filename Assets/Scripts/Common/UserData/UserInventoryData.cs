using System.Collections.Generic;
using UnityEngine;

public class UserInventoryData : IUserData
{
    List<Item> inventoryItems = new List<Item>();

    public bool LoadData()
    {
        throw new System.NotImplementedException();
    }

    public bool SaveData()
    {
        throw new System.NotImplementedException();
    }

    public void SetDefaultData()
    {
        throw new System.NotImplementedException();
    }
}
