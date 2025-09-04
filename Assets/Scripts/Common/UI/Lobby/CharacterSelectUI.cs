using Gpm.Ui;
using TMPro;
using UnityEngine;

public class CharacterSelectUI : BaseUI
{
    [SerializeField] InfiniteScroll characterScroll;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Transform instantiatedCharacterParent;

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

        characterScroll.SetSpace(new Vector2 (50, 50));

        SetCharacterScrollList();
        SetCharacterTextUI();

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

    private void SetCharacterTextUI()
    {
        if(selectedCharacterType != CharacterType.None)
        {
            characterNameText.text = selectedCharacterType.ToString();
        }
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
        SetCharacterTextUI();
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
