using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    [Header("Inventory UI")]
    [SerializeField] InfiniteScroll itemInfiniteScroll;
    [SerializeField] InfiniteScroll statInfiniteScroll;
    [SerializeField] RawImage PlayerUIRawImage;

    [Header("Equipped Item Slots")]
    [SerializeField] EquipmentSlot weaponItemSlot;
    [SerializeField] EquipmentSlot subWeaponItemSlot;
    [SerializeField] EquipmentSlot helmetItemSlot;
    [SerializeField] EquipmentSlot accessoryItemSlot;
    [SerializeField] EquipmentSlot potionItemSlot;
    [SerializeField] EquipmentSlot foodItemSlot;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        BindCharacterUI();
        SetInventoryItems();
        SetStatData();
        SetEquipmentData();
    }

    public void BindCharacterUI()
    {
        PlayerUIRawImage.texture = CharacterUIManager.Instance.GetUIPlayerRT(0);
    }

    public void SetInventoryItems()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("userInventoryData is Null");
            return;
        }
        var userInventoryList = userInventoryData.InventoryItems;
        itemInfiniteScroll.Clear();
        itemInfiniteScroll.SetSpace(new Vector2(20, 20)); 
        foreach (var item in userInventoryList)
        {
            if(item.IsEquipped)
                continue;
            var newItem = new InventorySlotUIData
            {
                SerialNumber = item.SerialNumber,
                ItemId = item.ItemID
            };

            itemInfiniteScroll.InsertData(newItem);
        }
    }

    public void SetStatData()
    {
        statInfiniteScroll.SetSpace(new Vector2(0, 0));
        var userdata = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if (userdata == null)
        {
            Logger.LogError("UserInventoryData is null");
            return;
        }
        var userInventoryList = userdata.InventoryItems;
        statInfiniteScroll.Clear();

        PlayerStat equippedItemStat = new PlayerStat(5f, 1f, 5f, 2f); //TODO: 나중에 캐릭터 종류에 따라 다른 스텟 입력

        foreach (var item in userInventoryList)
        {
            if (item.IsEquipped)
            {
                var itemData = DataTableManager.Instance.GetItemData(item.ItemID);

                if (itemData == null)
                {
                    Logger.LogError($"ItemData not found for item ID: {item.ItemID}");
                    continue;
                }

                foreach(var itemModifierKey in itemData.StatModifiers.Keys)
                {
                    switch(itemModifierKey)
                    {
                        case GlobalDefine.STAT_ATTACKDAMAGE_FLAT:
                            equippedItemStat.AttackDamage.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_ATTACKRANGE_FLAT:
                            equippedItemStat.AttackRange.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_MOVESPEED_FLAT:
                            equippedItemStat.MoveSpeed.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_ATTACKSPEED_FLAT:
                            equippedItemStat.AttackSpeed.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_CRITICALCHANCE_PERCENT:
                            equippedItemStat.CriticalChance.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_CRITICALDAMAGE_FLAT:
                            equippedItemStat.CriticalDamage.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_HEALTHPOINT_FLAT:
                            equippedItemStat.HealthPoint.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Flat, item));
                            break;
                        case GlobalDefine.STAT_ATTACKDAMAGE_PERCENT:
                            equippedItemStat.AttackDamage.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Percent, item));
                            break;
                        case GlobalDefine.STAT_ATTACKRANGE_PERCENT:
                            equippedItemStat.AttackRange.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Percent, item));
                            break;
                        case GlobalDefine.STAT_MOVESPEED_PERCENT:
                            equippedItemStat.MoveSpeed.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Percent, item));
                            break;
                        case GlobalDefine.STAT_ATTACKSPEED_PERCENT:
                            equippedItemStat.AttackSpeed.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Percent, item));
                            break;
                        case GlobalDefine.STAT_HEALTHPOINT_PERCENT:
                            equippedItemStat.HealthPoint.AddModifier(new StatModifier(itemData.StatModifiers[itemModifierKey].Value, ModifierType.Percent, item));
                            break;
                    }
                }

            }
        }

        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Move Speed",
            StatName = "이동속도",
            IsCriticalStat = false,
            CharacterStatAmount = 1f,
            FlatIncreasementAmount = equippedItemStat.MoveSpeed.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.MoveSpeed.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Attack Damage",
            StatName = "공격력",
            IsCriticalStat = false,
            CharacterStatAmount = 5f,
            FlatIncreasementAmount = equippedItemStat.AttackDamage.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackDamage.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Attack Range",
            StatName = "공격 범위",
            IsCriticalStat = false,
            CharacterStatAmount = 1f,
            FlatIncreasementAmount = equippedItemStat.AttackRange.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackRange.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Attack Speed",
            StatName = "공격 속도",
            IsCriticalStat = false,
            CharacterStatAmount = 5f,
            FlatIncreasementAmount = equippedItemStat.AttackSpeed.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackSpeed.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Critical Chance",
            StatName = "치명타 확률",
            IsCriticalStat = true,
            CharacterStatAmount = 0.05f,
            FlatIncreasementAmount = equippedItemStat.CriticalChance.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.CriticalChance.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "Critical Damage",
            StatName = "치명타 피해",
            IsCriticalStat = true,
            CharacterStatAmount = 1.1f,
            FlatIncreasementAmount = equippedItemStat.CriticalDamage.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.CriticalDamage.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatImageName = "HP",
            StatName = "체력",
            IsCriticalStat = false,
            CharacterStatAmount = 50f,
            FlatIncreasementAmount = equippedItemStat.HealthPoint.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.HealthPoint.GetAllPercentModifierSum()
        });
    }

    public void SetEquipmentData()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if (userInventoryData == null)
        {
            Logger.LogError("userInventoryData is Null");
            return;
        }
        var userInventoryList = userInventoryData.InventoryItems;

        {
            weaponItemSlot.ResetItem();
            subWeaponItemSlot.ResetItem();
            helmetItemSlot.ResetItem();
            potionItemSlot.ResetItem();
            accessoryItemSlot.ResetItem();
            foodItemSlot.ResetItem();

        } //장비 장착칸 초기화

        foreach (var item in userInventoryList)
        {
            if (item.IsEquipped)
            {
                var itemData = DataTableManager.Instance.GetItemData(item.ItemID);

                switch (itemData.ItemEquipType)
                {
                    case ItemEquipType.Weapon:
                        weaponItemSlot.SetItem(item.SerialNumber);
                        break;
                    case ItemEquipType.SubWeapon:
                        subWeaponItemSlot.SetItem(item.SerialNumber);
                        break;
                    case ItemEquipType.Helmet:
                        helmetItemSlot.SetItem(item.SerialNumber);
                        break;
                    case ItemEquipType.Potion:
                        potionItemSlot.SetItem(item.SerialNumber);
                        break;
                    case ItemEquipType.Accessory:
                        accessoryItemSlot.SetItem(item.SerialNumber);
                        break;
                    case ItemEquipType.Food:
                        foodItemSlot.SetItem(item.SerialNumber);
                        break;

                } 
            }
        }//Inventory를 순회하여 장착된 아이템 UI Icon로드
    }

    public void RefreshInventoryUI()
    {
        SetInventoryItems();
        SetStatData();
        SetEquipmentData();
    }

    public void OnClickGameStartButton()
    {
        UIManager.Instance.CloseAllUI();
        LobbyManager.Instance.StartInGame();
    }

    public void OnClickCharacterChangeButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<CharacterSelectUI>(uiData);
    }
}
