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
    public readonly int itemID;
    public readonly string itemName;
    public readonly ItemEquipType itemEquipType;
    public readonly ItemRarity itemRarity;
    public readonly IReadOnlyDictionary<string, ItemStatData> StatModifiers;

    public ItemData(int itemID, string itemName, ItemEquipType itemEquipType, ItemRarity itemRarity, Dictionary<string, ItemStatData> statModifiers)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemEquipType = itemEquipType;
        this.itemRarity = itemRarity;
        StatModifiers = statModifiers;
    }
}

public class ItemStatData
{
    public readonly float value;
    public readonly ModifierType modifierType;

    public ItemStatData(float value, ModifierType type)
    {
        this.value = value;
        this.modifierType = type;
    }
}

[Serializable]
public class Item
{
    public long serialNumber;
    public int itemID;
    public int abilityID;
    public bool isEquipped;
    public Dictionary<string, StatModifier> appliedStats = new Dictionary<string, StatModifier>();

    private ItemData itemData = null;
    public ItemData ItemData
    {
        get
        {
            if (itemData == null)
            {
                itemData = DataTableManager.Instance.GetItemData(itemID);
                if(itemData == null)
                {
                    Logger.LogError($"ItemID {itemID} not found in DataTableManager");
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
        this.serialNumber = serialNumber;
        this.itemID = itemID;
        this.isEquipped = isEquipped;

        this.abilityID = abilityID;

        if(ItemData == null)
        {
            Logger.LogError($"{itemID} item cannot load ItemData");
        }
        else
        {
            appliedStats.Clear();
            foreach (var modifier in ItemData.StatModifiers)
            {
                appliedStats.Add(modifier.Key, new StatModifier(modifier.Value.value, modifier.Value.modifierType, this));
            }
        }
    }

    public void ExecuteSkill(PlayerContext ctx)
    {
        if (abilityID != 0)
        {
            //ability.ExecuteSkill(ctx);
        }
    }

    public void Equip(Player player)
    {
        if (isEquipped) return;
        
        var stat = player.PlayerStat;

        foreach (var modifier in appliedStats)
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

        isEquipped = true;
    }

    public void UnEquip(Player player)
    {
        if (!isEquipped) return;
        var stat = player.PlayerStat;
        object source = this;

        stat.MoveSpeed.RemoveModifier(source);
        stat.AttackRange.RemoveModifier(source);
        stat.AttackDamage.RemoveModifier(source);
        stat.AttackSpeed.RemoveModifier(source);
        stat.CriticalChance.RemoveModifier(source);
        stat.CriticalDamage.RemoveModifier(source);
        stat.HealthPoint.RemoveModifier(source);

        isEquipped = false;

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
