using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUIItemData : InfiniteScrollData
{
    public CharacterType CharacterType;
}

public class CharacterSelectUIItem : InfiniteScrollItem
{
    [SerializeField] Transform CharacterPosParent;

    private CharacterSelectUIItemData characterSelectUIItemData;
    private GameObject playerCardInstance;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        characterSelectUIItemData = scrollData as CharacterSelectUIItemData;
        if(characterSelectUIItemData == null)
        {
            Logger.LogError("characterSelectUIItemData is invalid.");
            return;
        }

        if(playerCardInstance == null)
        {
            SetPlayerCharacterCard();
        }
        if(playerCardInstance != null)
        {
            playerCardInstance.SetActive(true);
            playerCardInstance.transform.localPosition = Vector3.zero;
            playerCardInstance.transform.localScale = Vector3.one * 300;
        }
        
    }

    private void SetPlayerCharacterCard()
    {
        LayerMask UILayer = LayerMask.NameToLayer("UI");

        var characterPrefab = Resources.Load<GameObject>($"Units/Dummy_{(int)characterSelectUIItemData.CharacterType}");
        if (characterPrefab == null)
        {
            Logger.LogError($"Character Prefab is null. CharacterType : {characterSelectUIItemData.CharacterType}");
            playerCardInstance = null;
        }
        else
        {
            var obj = Instantiate(characterPrefab, CharacterPosParent);
            obj.layer = UILayer;
            SetLayerRecursively(obj, UILayer);
            playerCardInstance = obj;

        }
    }

    private void SetLayerRecursively(GameObject gameObject, LayerMask layer)
    {
        gameObject.layer = layer;
        foreach (Transform trans in gameObject.transform)
        {
            SetLayerRecursively(trans.gameObject, layer);
        }
    }

}
