using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectUI : BaseUI
{
    [SerializeField] InfiniteScroll characterScroll;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Transform instantiatedCharacterParent;

    private List<GameObject> instantiatedCharacterList = new List<GameObject>();
    private bool isInstantiated = false;
    private CharacterType selectedCharacterType = CharacterType.None;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        int playerSelectIndex = (int)userPlayData.SelectedCharacter;
        if (playerSelectIndex <= 0 || playerSelectIndex >= (int)CharacterType.Count)
        {
            playerSelectIndex = 1;
        }

        characterScroll.SetSpace(new Vector2 (50, 0));
        characterScroll.Clear();
        SetCharacterScrollList();

        selectedCharacterType = (CharacterType)playerSelectIndex;
        characterScroll.MoveTo(playerSelectIndex - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);

        characterScroll.OnSnap = (currentSnappedIndex) =>
        {
            selectedCharacterType = (CharacterType)(currentSnappedIndex + 1);
            SetCharacterTextUI();
        };

    }

    private void SetCharacterTextUI()
    {
        if(selectedCharacterType != CharacterType.None)
        {
            characterNameText.text = selectedCharacterType.ToString();
        }
    }

    private void SetCharacterScrollList()
    {
        if(isInstantiated == false)
        {
            InstantiateCharacterPrefabs();
        }
        for (int i = 0; i < (int)CharacterType.Count; i++)
        {
            CharacterSelectUIItemData characterData = new CharacterSelectUIItemData();
            characterData.CharacterType = (CharacterType)i;
            characterData.InstantiatedPrefabs = instantiatedCharacterList[i];
            characterScroll.InsertData(characterData);
        }

    }

    private void InstantiateCharacterPrefabs()
    {
        for (int i = 0; i < (int)CharacterType.Count; i++)
        {
            var characterPrefab = Resources.Load<GameObject>($"Units/Dummy_{i}");
            if(characterPrefab == null)
            {
                Logger.LogError($"Character Prefab is null. CharacterType : {(CharacterType)i}");
                return;
            }
            else
            {
                var obj = Instantiate(characterPrefab, instantiatedCharacterParent);
                characterPrefab.SetActive(false);
                instantiatedCharacterList.Add(obj);
            }
        }
        isInstantiated = true;
    }

    public void OnClickConfirmButton()
    {
        if (selectedCharacterType != CharacterType.None)
        {
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if(userPlayData == null)
            {
                Logger.LogError("userPlayData is null.");
            }
            else
            {
                userPlayData.SelectedCharacter = selectedCharacterType;
                userPlayData.SaveData();
            }
        }
        CloseUI();
    }

    public override void CloseUI(bool isCloseAll = false)
    {
        base.CloseUI(isCloseAll);
        foreach(var obj in instantiatedCharacterList)
        {
            obj.SetActive(false);
            obj.transform.SetParent(instantiatedCharacterParent, false);
        }
    }
}
