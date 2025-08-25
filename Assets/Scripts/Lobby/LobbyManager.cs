using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        
    }

    public void StartInGame()
    {
        UIManager.Instance.CloseAllUI();

        SceneLoader.Instance.LoadScene(SceneType.InGame);
    }
}
