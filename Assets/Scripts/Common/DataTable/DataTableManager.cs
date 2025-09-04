using System;
using System.Collections.Generic;
using System.Text;

public class CharacterData
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

    public CharacterData(int characterID, string characterName, float moveSpeed, float attackDamage, float attackRange, float attackSpeed, float critChance, float critDamage, float healthPoint)
    {
        CharacterID = characterID;
        CharacterName = characterName;
        MoveSpeed = moveSpeed;
        AttackDamage = attackDamage;
        AttackRange = attackRange;
        AttackSpeed = attackSpeed;
        CritChance = critChance;
        CritDamage = critDamage;
        HP = healthPoint;
    }
}

public class DataTableManager : Singleton<DataTableManager>
{
    private const string DATA_TABLE_PATH = "DataTables";
    
    protected override void Init()
    {
        base.Init();

        LoadItemDataTable();
        LoadCharacterData();
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

    private List<CharacterData> characterDataList = new();

    private void LoadCharacterData()
    {
        var readData = CSVReader.Read($"{DATA_TABLE_PATH}/CharacterDataTable");
        StringBuilder sb = new();

        foreach (var item in readData)
        {
            var charData = new CharacterData(
                characterID: Convert.ToInt32(item["character_id"]),
                characterName: item["character_name"].ToString(),
                moveSpeed: Convert.ToSingle(item["move_speed"]),
                attackDamage: Convert.ToSingle(item["attack_damage"]),
                attackRange: Convert.ToSingle(item["attack_range"]),
                attackSpeed: Convert.ToSingle(item["attack_speed"]),
                critChance: Convert.ToSingle(item["crit_chance"]),
                critDamage: Convert.ToSingle(item["crit_damage"]),
                healthPoint: Convert.ToSingle(item["health_point"])
                );

            characterDataList.Add(charData);
        }
    }

    public CharacterData GetCharacterData(int charID)
    {
        return characterDataList.Find(cd => cd.CharacterID == charID);
    }

    public CharacterData GetCharacterData(CharacterType type)
    {
        if (type == CharacterType.None)
        {
            Logger.LogError("Can not Get None Type CharacterData");
            return null;
        }
        return characterDataList.Find(cd => cd.CharacterID - 1 == (int)type);
    }

    #endregion
}
