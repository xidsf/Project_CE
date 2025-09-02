using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUIItemData : InfiniteScrollData
{
    public CharacterType CharacterType;
}

public class CharacterSelectUIItem : InfiniteScrollItem
{
    [SerializeField] Transform characterPrefabParent;

    private CharacterSelectUIItemData characterSelectUIItemData;
    private static GameObject[] playerPrefabs = null;
    private static bool isAllPrefabsInstantiated = false;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        if(!isAllPrefabsInstantiated)
        {
            playerPrefabs = new GameObject[(int)CharacterType.Count];
            for (int i = 0; i < (int)CharacterType.Count; i++)
            {
                var obj = Resources.Load<GameObject>($"Units/Dummy_{i}");
                if(obj != null)
                {
                    playerPrefabs[i] = Instantiate(obj);
                    playerPrefabs[i].GetComponent<RectTransform>().localScale = Vector3.one * 50f;
                    playerPrefabs[i].SetActive(false);
                }
                else
                {
                    Logger.LogError($"Failed to load prefab for {(CharacterType)i}");
                    isAllPrefabsInstantiated = false;
                    return;
                }
            }
            isAllPrefabsInstantiated = true;
        }

        characterSelectUIItemData = scrollData as CharacterSelectUIItemData;
        if(characterSelectUIItemData == null)
        {
            Logger.LogError("characterSelectUIItemData is invalid.");
            return;
        }

        playerPrefabs[(int)characterSelectUIItemData.CharacterType].SetActive(true);
        playerPrefabs[(int)characterSelectUIItemData.CharacterType].transform.SetParent(characterPrefabParent, false);
        playerPrefabs[(int)characterSelectUIItemData.CharacterType].transform.localPosition = Vector3.zero;
    }
}
