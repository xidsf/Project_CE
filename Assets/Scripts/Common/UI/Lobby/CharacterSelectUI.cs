using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUI : BaseUI
{
    [SerializeField] InfiniteScroll characterScroll;

    private CharacterType selectedCharacterType = CharacterType.None;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        int playerSelectIndex = (int)userPlayData.SelectedCharacter;
        if (playerSelectIndex <= 0 || playerSelectIndex >= (int)CharacterType.Count)
        {
            playerSelectIndex = 0;
        }

        characterScroll.Clear();
        characterScroll.SetPadding(new Vector2(300, 0));
        characterScroll.SetSpace(new Vector2(200, 50));

        SetCharacterScrollList();

        selectedCharacterType = (CharacterType)playerSelectIndex;
        characterScroll.MoveTo(playerSelectIndex, InfiniteScroll.MoveToType.MOVE_TO_CENTER);

        characterScroll.OnSnap = (currentSnappedIndex) =>
        {
            var CharacterSelectUI = UIManager.Instance.GetActiveUI<CharacterSelectUI>() as CharacterSelectUI;
            if (CharacterSelectUI != null)
            {
                CharacterSelectUI.OnSnap(currentSnappedIndex);
            }
        };

    }


    private void SetCharacterScrollList()
    {
        characterScroll.Clear();
        for (int i = 0; i < (int)CharacterType.Count; i++)
        {
            CharacterSelectUIItemData characterData = new CharacterSelectUIItemData();
            characterData.CharacterType = (CharacterType)i;
            characterScroll.InsertData(characterData);
        }

    }
    

    public void OnSnap(int snappedIndex)
    {
        selectedCharacterType = (CharacterType)snappedIndex;
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

}
