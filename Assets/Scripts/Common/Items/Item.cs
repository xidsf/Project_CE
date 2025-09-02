using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEquipType
{
    Weapon = 1,
    SubWeapon,
    Potion,
    Helmet,
    Accessory,
    Food,
    COUNT,
}

public enum ItemRarity
{
    Normal = 1,
    Rare,
    Epic,
    Unique,
    Legendary
}

public class ItemData
{
    public readonly int ItemID;
    public readonly string ItemName;
    public readonly ItemEquipType ItemEquipType;
    public readonly ItemRarity ItemRarity;
    public readonly IReadOnlyDictionary<string, ItemStatData> StatModifiers;

    public ItemData(int itemID, string itemName, ItemEquipType itemEquipType, ItemRarity itemRarity, Dictionary<string, ItemStatData> statModifiers)
    {
        ItemID = itemID;
        ItemName = itemName;
        ItemEquipType = itemEquipType;
        ItemRarity = itemRarity;
        StatModifiers = statModifiers;
    }
}

public class ItemStatData
{
    public readonly float Value;
    public readonly ModifierType ModifierType;

    public ItemStatData(float value, ModifierType type)
    {
        Value = value;
        ModifierType = type;
    }
}

[Serializable]
public class Item
{
    public long SerialNumber;
    public int ItemID;
    public int AbilityID;
    public bool IsEquipped;
    public Dictionary<string, StatModifier> AppliedStats = new Dictionary<string, StatModifier>();

    private ItemData itemData = null;
    public ItemData ItemData
    {
        get
        {
            if (itemData == null)
            {
                itemData = DataTableManager.Instance.GetItemData(ItemID);
                if(itemData == null)
                {
                    Logger.LogError($"ItemID {ItemID} not found in DataTableManager");
                    return null;
                }
                else
                {
                    return itemData;
                }
            }
            else
            {
                return itemData;
            }
            
        }
    }

    public Item(long serialNumber, int itemID, bool isEquipped = false, int abilityID = 0)
    {
        SerialNumber = serialNumber;
        ItemID = itemID;
        IsEquipped = isEquipped;
        AbilityID = abilityID;

        if(ItemData == null)
        {
            Logger.LogError($"{itemID} item cannot load ItemData");
        }
        else
        {
            AppliedStats.Clear();
            foreach (var modifier in ItemData.StatModifiers)
            {
                AppliedStats.Add(modifier.Key, new StatModifier(modifier.Value.Value, modifier.Value.ModifierType, this));
            }
        }
    }

    public void ExecuteSkill(PlayerContext ctx)
    {
        if (AbilityID != 0)
        {
            //ability.ExecuteSkill(ctx);
        }
    }

    public void Equip(Player player)
    {
        if (IsEquipped) return;
        
        var stat = player.PlayerStat;

        foreach (var modifier in AppliedStats)
        {
            if (modifier.Value.modifierType == ModifierType.Flat)
            {
                switch(modifier.Key)
                {
                    case GlobalDefine.STAT_MOVESPEED_FLAT:
                        stat.MoveSpeed.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKRANGE_FLAT:
                        stat.AttackRange.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKDAMAGE_FLAT:
                        stat.AttackDamage.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKSPEED_FLAT:
                        stat.AttackSpeed.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_CRITICALDAMAGE_FLAT:
                        stat.CriticalDamage.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_HEALTHPOINT_FLAT:
                        stat.HealthPoint.AddModifier(modifier.Value);
                        break;
                    default:
                        Debug.LogWarning($"Unknown flat modifier key: {modifier.Key}");
                        break;
                }

            }
            else if (modifier.Value.modifierType == ModifierType.Percent)
            {
                switch(modifier.Key)
                {
                    case GlobalDefine.STAT_MOVESPEED_PERCENT:
                        stat.MoveSpeed.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKRANGE_PERCENT:
                        stat.AttackRange.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKDAMAGE_PERCENT:
                        stat.AttackDamage.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_ATTACKSPEED_PERCENT:
                        stat.AttackSpeed.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_HEALTHPOINT_PERCENT:
                        stat.HealthPoint.AddModifier(modifier.Value);
                        break;
                    case GlobalDefine.STAT_CRITICALCHANCE_PERCENT:
                        stat.CriticalChance.AddModifier(modifier.Value);
                        break;
                    default:
                        Debug.LogWarning($"Unknown percent modifier key: {modifier.Key}");
                        break;
                }
            }
        }

        IsEquipped = true;
    }

    public void UnEquip(Player player)
    {
        if (!IsEquipped) return;
        var stat = player.PlayerStat;
        object source = this;

        stat.MoveSpeed.RemoveModifier(source);
        stat.AttackRange.RemoveModifier(source);
        stat.AttackDamage.RemoveModifier(source);
        stat.AttackSpeed.RemoveModifier(source);
        stat.CriticalChance.RemoveModifier(source);
        stat.CriticalDamage.RemoveModifier(source);
        stat.HealthPoint.RemoveModifier(source);

        IsEquipped = false;

    }

    public static Color GetRarityColor(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Normal => Color.grey,
            ItemRarity.Rare => Color.blue,
            ItemRarity.Epic => Color.magenta,
            ItemRarity.Unique => Color.yellow,
            ItemRarity.Legendary => Color.red,
            _ => Color.white,
        };
    }
}
