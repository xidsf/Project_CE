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
    public int itemID;
    public string itemName;
    public ItemEquipType itemEquipType;
    public ItemRarity itemRarity;

    public Dictionary<string, StatModifier> StatModifiers = new();
}

[Serializable]
public class Item
{
    public long serialNumber;
    public int itemID;
    public int abilityID;
    public bool isEquipped;

    private ItemData itemData = null;
    public ItemData ItemData
    {
        get
        {
            if (itemData == null)
                itemData = DataTableManager.Instance.GetItemData(itemID);
            return itemData;
        }
        private set => itemData = value;
    }

    public Item(long serialNumber, int itemID, bool isEquipped = false, int abilityID = 0)
    {
        this.serialNumber = serialNumber;
        this.itemID = itemID;
        this.isEquipped = isEquipped;

        this.abilityID = abilityID;

        ItemData = DataTableManager.Instance.GetItemData(itemID);

        if(ItemData == null)
        {
            Logger.LogError($"{itemID} item cannot load ItemData");
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
        object source = this;

        foreach (var modifier in ItemData.StatModifiers)
        {
            if (modifier.Value.modifierType == ModifierType.Flat)
            {
                switch(modifier.Key)
                {
                    case GlobalDefine.STAT_MOVESPEED_FLAT:
                        stat.MoveSpeed.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
                        break;
                    case GlobalDefine.STAT_ATTACKRANGE_FLAT:
                        stat.AttackRange.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
                        break;
                    case GlobalDefine.STAT_ATTACKDAMAGE_FLAT:
                        stat.AttackDamage.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
                        break;
                    case GlobalDefine.STAT_ATTACKSPEED_FLAT:
                        stat.AttackSpeed.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
                        break;
                    case GlobalDefine.STAT_CRITICALDAMAGE_FLAT:
                        stat.CriticalDamage.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
                        break;
                    case GlobalDefine.STAT_HEALTHPOINT_FLAT:
                        stat.HealthPoint.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Flat, source));
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
                        stat.MoveSpeed.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
                        break;
                    case GlobalDefine.STAT_ATTACKRANGE_PERCENT:
                        stat.AttackRange.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
                        break;
                    case GlobalDefine.STAT_ATTACKDAMAGE_PERCENT:
                        stat.AttackDamage.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
                        break;
                    case GlobalDefine.STAT_ATTACKSPEED_PERCENT:
                        stat.AttackSpeed.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
                        break;
                    case GlobalDefine.STAT_HEALTHPOINT_PERCENT:
                        stat.HealthPoint.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
                        break;
                    case GlobalDefine.STAT_CRITICALCHANCE_PERCENT:
                        stat.CriticalChance.AddModifier(new StatModifier(modifier.Value.value, ModifierType.Percent, source));
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
