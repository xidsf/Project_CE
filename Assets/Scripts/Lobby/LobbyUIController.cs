using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public Button GameStartButton;
    public Button GameSettingsButton;
    public Button GameEndButton;

    private void Start()
    {
        GameStartButton.interactable = false;
        GameSettingsButton.interactable = false;
        GameEndButton.interactable = false;

        StartCoroutine(LobbyUICoroutine());
    }

    IEnumerator LobbyUICoroutine()
    {
        yield return new WaitForSeconds(2);
        GameStartButton.interactable = true;
        GameSettingsButton.interactable = true;
        GameEndButton.interactable = true;
    }

    public void OnClickStartButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    public void OnClickEndButton()
    {
        Application.Quit();
    }
}
