using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUIItemData : InfiniteScrollData
{
    public CharacterType CharacterType;
    public GameObject InstantiatedPrefabs;
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

        var playerPrefabs = characterSelectUIItemData.InstantiatedPrefabs;

        playerPrefabs.SetActive(true);
        playerPrefabs.transform.SetParent(characterPrefabParent, false);
        playerPrefabs.transform.localPosition = Vector3.zero;
    }
}
