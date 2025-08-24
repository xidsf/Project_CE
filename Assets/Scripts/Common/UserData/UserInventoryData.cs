using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UserInventoryDataWrapper
{
    public List<Item> inventoryItems;
}

public class UserInventoryData : IUserData
{
    public List<Item> InventoryItems { get; private set; } = new List<Item>();

    public void SetDefaultData()
    {
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 113000, true));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 212000));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 211020));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 213006));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 214007));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 215011));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 125017));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 135018));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 311000));
        InventoryItems.Add(new Item(SerialNumberGenerator.GenerateSerialNumber(), 331012));
    }

    public bool LoadData()
    {
        bool result = false;

        try
        {
            var wrapper = PlayerPrefs.GetString("UserInventoryData");
            if (!string.IsNullOrEmpty(wrapper))
            {
                var loadedData = JsonUtility.FromJson<UserInventoryDataWrapper>(wrapper);
                InventoryItems = loadedData.inventoryItems;
                result = true;
            }

        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load user inventory data: {e.Message}");
        }
        return result;
    }

    public bool SaveData()
    {
        bool result = false;

        try
        {
            var wrapper = new UserInventoryDataWrapper();
            wrapper.inventoryItems = InventoryItems;
            string jsonData = JsonUtility.ToJson(wrapper);
            if (!string.IsNullOrEmpty(jsonData))
            {
                PlayerPrefs.SetString("UserInventoryData", jsonData);
                PlayerPrefs.Save();
                result = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save user inventory data: {e.Message}");
        }
        return result;
    }

    public void EquipItem(long serialNum)
    {
        var equippingItem = InventoryItems.Find(item => item.serialNumber == serialNum);

        Item alreadyEquippedItem = InventoryItems.FirstOrDefault(item => item.ItemData.itemEquipType == equippingItem.ItemData.itemEquipType && item.isEquipped);

        if (alreadyEquippedItem != default)
        {
            alreadyEquippedItem.isEquipped = false;
        }

        if(equippingItem == default)
        {
            Logger.LogError($"Item with serial number {serialNum} not found in inventory.");
        }
        equippingItem.isEquipped = true;
        SaveData();
    }

    public void UnequipItem(long serialNum)
    {
        var foundItem = InventoryItems.Find(item => item.serialNumber == serialNum);
        if (foundItem != null)
        {
            foundItem.isEquipped = false;
        }

        SaveData();
    }
}
