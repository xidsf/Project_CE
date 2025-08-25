using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : Singleton<TitleManager>
{
    public GameObject titleLogoUI;
    public Animation titleLogoUIAnim;

    private AsyncOperation asyncOp;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        titleLogoUI.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(TitleLoadCoroutine());

        UserDataManager.Instance.LoadUserData();
        if(!UserDataManager.Instance.IsExistSavedData)
        {
            UserDataManager.Instance.SetDefaultData();
        }
    }

    private IEnumerator TitleLoadCoroutine()
    {
        titleLogoUIAnim.Play();
        yield return new WaitForSeconds(titleLogoUIAnim.clip.length);

        titleLogoUI.SetActive(false);

        asyncOp = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);

        if(asyncOp == null)
        {
            Logger.LogError($"{GetType()}::Missing asyncOperation");
            yield break;
        }

        asyncOp.allowSceneActivation = false;

        var uiData = new LoadingUIData() //AsyncOperation을 넘겨주는 부분은 별로인거 같은데 좋은 방법이 떠오르지 않음;;
        {
            AsyncOperation = asyncOp,
            fadeDuration = 0.5f,
        };
        UIManager.Instance.OpenUI<LoadingUI>(uiData);

        while(!asyncOp.isDone) yield return null;

    }
}
