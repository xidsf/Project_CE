using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectUI : BaseUI
{
    [SerializeField] InfiniteScroll characterScroll;
    [SerializeField] TextMeshProUGUI characterNameText;

    private UserPlayData userPlayData;
    private CharacterType selectedCharacterType = CharacterType.None;
    private List<GameObject> playerPrefabs = new();

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        int playerSelectIndex = (int)userPlayData.SelectedCharacter;
        if(playerSelectIndex <= 0 || playerSelectIndex >= (int)CharacterType.Count)
        {
            playerSelectIndex = 1;
        }
        selectedCharacterType = (CharacterType)playerSelectIndex;
        characterScroll.MoveTo(playerSelectIndex);

        for(int i = 0; i < (int)CharacterType.Count; i++)
        {
            var prefab = Resources.Load<GameObject>($"Prefabs/Units/Dummy_{(CharacterType)i}");
            if(prefab != null)
            {
                playerPrefabs.Add(prefab);
            }
        }

        //characterScroll.OnSnap =>
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
        for (int i = 0; i < (int)CharacterType.Count; i++)
        {
        }
    }

}
