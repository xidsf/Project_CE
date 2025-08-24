using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseUI
{
    [Header("Inventory UI")]
    [SerializeField] InfiniteScroll itemInfiniteScroll;
    [SerializeField] InfiniteScroll statInfiniteScroll;

    [Header("Equipped Item Slots")]
    [SerializeField] GameObject weaponEmptyIcon;
    [SerializeField] EquippedItemSlot weaponItemSlot;

    [SerializeField] GameObject subWeaponEmptyIcon;
    [SerializeField] EquippedItemSlot subWeaponItemSlot;

    [SerializeField] GameObject helmetEmptyIcon;
    [SerializeField] EquippedItemSlot helmetItemSlot;

    [SerializeField] GameObject potionEmptyIcon;
    [SerializeField] EquippedItemSlot potionItemSlot;

    [SerializeField] GameObject accessoryEmptyIcon;
    [SerializeField] EquippedItemSlot accessoryItemSlot;

    [SerializeField] GameObject foodEmptyIcon;
    [SerializeField] EquippedItemSlot foodItemSlot;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        SetInventoryItems();
        SetStatData();
        SetEquipmentData();
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
            if(item.isEquipped)
                continue;
            var newItem = new InventorySlotUIData
            {
                serialNumber = item.serialNumber,
                ItemId = item.itemID
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
            if (item.isEquipped)
            {
                var itemData = DataTableManager.Instance.GetItemData(item.itemID);
                foreach(var modifier in itemData.StatModifiers)
                {

                }
            }
        }

        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Move Speed",
            IsCriticalStat = false,
            CharacterStatAmount = 1f,
            FlatIncreasementAmount = equippedItemStat.MoveSpeed.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.MoveSpeed.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Attack Damage",
            IsCriticalStat = false,
            CharacterStatAmount = 5f,
            FlatIncreasementAmount = equippedItemStat.AttackDamage.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackDamage.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Attack Range",
            IsCriticalStat = false,
            CharacterStatAmount = 1f,
            FlatIncreasementAmount = equippedItemStat.AttackRange.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackRange.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Attack Speed",
            IsCriticalStat = false,
            CharacterStatAmount = 5f,
            FlatIncreasementAmount = equippedItemStat.AttackSpeed.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.AttackSpeed.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Critical Chance",
            IsCriticalStat = true,
            CharacterStatAmount = 0.05f,
            FlatIncreasementAmount = equippedItemStat.CriticalChance.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.CriticalChance.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Critical Damage",
            IsCriticalStat = true,
            CharacterStatAmount = 1.1f,
            FlatIncreasementAmount = equippedItemStat.CriticalDamage.GetAllFlatModifierSum(),
            PercentIncreasementAmount = equippedItemStat.CriticalDamage.GetAllPercentModifierSum()
        });
        statInfiniteScroll.InsertData(new StatSlotUIData
        {
            StatName = "Health Point",
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

            weaponEmptyIcon.SetActive(true);
            subWeaponEmptyIcon.SetActive(true);
            helmetEmptyIcon.SetActive(true);
            potionEmptyIcon.SetActive(true);
            accessoryEmptyIcon.SetActive(true);
            foodEmptyIcon.SetActive(true);

            weaponItemSlot.gameObject.SetActive(false);
            subWeaponItemSlot.gameObject.SetActive(false);
            helmetItemSlot.gameObject.SetActive(false);
            potionItemSlot.gameObject.SetActive(false);
            accessoryItemSlot.gameObject.SetActive(false);
            foodItemSlot.gameObject.SetActive(false);

        } //장비 장착칸 초기화

        foreach (var item in userInventoryList)
        {
            if (item.isEquipped)
            {
                var itemData = DataTableManager.Instance.GetItemData(item.itemID);

                switch (itemData.itemEquipType)
                {
                    case ItemEquipType.Weapon:
                        weaponEmptyIcon.SetActive(false);
                        weaponItemSlot.gameObject.SetActive(true);
                        weaponItemSlot.SetItem(item.serialNumber);
                        break;
                    case ItemEquipType.SubWeapon:
                        subWeaponEmptyIcon.SetActive(false);
                        subWeaponItemSlot.gameObject.SetActive(true);
                        subWeaponItemSlot.SetItem(item.serialNumber);
                        break;
                    case ItemEquipType.Helmet:
                        helmetEmptyIcon.SetActive(false);
                        helmetItemSlot.gameObject.SetActive(true);
                        helmetItemSlot.SetItem(item.serialNumber);
                        break;
                    case ItemEquipType.Potion:
                        potionEmptyIcon.SetActive(false);
                        potionItemSlot.gameObject.SetActive(true);
                        potionItemSlot.SetItem(item.serialNumber);
                        break;
                    case ItemEquipType.Accessory:
                        accessoryEmptyIcon.SetActive(false);
                        accessoryItemSlot.gameObject.SetActive(true);
                        accessoryItemSlot.SetItem(item.serialNumber);
                        break;
                    case ItemEquipType.Food:
                        foodEmptyIcon.SetActive(false);
                        foodItemSlot.gameObject.SetActive(true);
                        foodItemSlot.SetItem(item.serialNumber);
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

}
