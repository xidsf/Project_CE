using System.Collections;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    bool isInGameLoading = false;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();
    }

    public void StartInGame()
    {
        if(isInGameLoading) return;
        isInGameLoading = true;
        StartCoroutine(LoadInGameCoroutine());
    }

    IEnumerator LoadInGameCoroutine()
    {
        AsyncOperation async = SceneLoader.Instance.LoadSceneAsync(SceneType.InGame);
        var uiData = new LoadingUIData()
        {
            AsyncOperation = async,
            FadeDuration = 0.5f,
        };
        UIManager.Instance.OpenUI<LoadingUI>(uiData);
        while (async.isDone)
        {
            yield return null;
        }
    }
}
