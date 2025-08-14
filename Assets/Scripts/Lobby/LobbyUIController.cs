using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public Button GameStartButton;

    private void Start()
    {
        GameStartButton.interactable = false;

        StartCoroutine(LobbyUICoroutine());
    }

    IEnumerator LobbyUICoroutine()
    {
        yield return new WaitForSeconds(2);
        GameStartButton.interactable = true;
    }

    public void OnClickStartButton()
    {
        LobbyManager.Instance.StartInGame();
    }
}
