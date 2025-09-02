using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUIItemData : InfiniteScrollData
{
    public CharacterType CharacterType;
    public string CharacterName;
}

public class CharacterSelectUIItem : InfiniteScrollItem
{
    [SerializeField] Transform characterPrefabParent;

    private CharacterSelectUIItemData characterSelectUIItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        characterSelectUIItemData = scrollData as CharacterSelectUIItemData;
        if(characterSelectUIItemData == null)
        {
            Logger.LogError("characterSelectUIItemData is invalid.");
            return;
        }

    }
}
