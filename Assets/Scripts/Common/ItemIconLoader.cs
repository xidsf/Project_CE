using System.Linq;
using UnityEngine;

public static class ItemIconLoader
{
    public static bool LoadItemIcon(int itemID, out Sprite itemIcon)
    {
        string itemFileFirstName = GetItemFileFirstName(itemID);
        var itemSprites = Resources.LoadAll<Sprite>($"Textures/ItemIcons/{itemFileFirstName}");

        if (itemSprites == null || itemSprites.Length == 0)
        {
            Logger.LogError($"No sprites found for item: {itemFileFirstName}");
            itemIcon = null;
            return false;
        }

        int num = itemID % 1000;
        itemIcon = itemSprites.FirstOrDefault(s => $"{itemFileFirstName}_{num.ToString()}" == s.name);
        if (itemIcon == null)
        {
            Logger.LogError($"Sprite not found for {itemFileFirstName}_{num} in Textures/ItemIcons/{itemFileFirstName}.");
            return false;
        }
        return true;
    }


    private static string GetItemFileFirstName(int itemID)
    {
        ItemEquipType itemEquipType = (ItemEquipType)(itemID / 1000 % 10);
        int jobNum = itemID / 100000;
        if (itemEquipType == ItemEquipType.Accessory)
        {
            return "Accessory";
        }
        else if (itemEquipType == ItemEquipType.Helmet)
        {
            return "Helmet";
        }
        else if (itemEquipType == ItemEquipType.Potion)
        {
            return "Potion";
        }
        else if (itemEquipType == ItemEquipType.SubWeapon)
        {
            if (jobNum == 2)
            {
                return "Shield";
            }
            else if (jobNum == 3)
            {
                return "Tome";
            }
        }
        else if (itemEquipType == ItemEquipType.Weapon)
        {
            if (jobNum == 2)
            {
                return "Sword";
            }
            else if (jobNum == 3)
            {
                return "Staff";
            }
        }
        return "Unknown Item";
    }

}
