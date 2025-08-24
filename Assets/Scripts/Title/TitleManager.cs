using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : Singleton<TitleManager>
{
    public GameObject titleLogoUI;
    public Animation titleLogoUIAnim;
    public GameObject loadingUI;
    public Slider loadingSlider;

    private AsyncOperation asyncOp;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        titleLogoUI.SetActive(true);
        loadingUI.SetActive(false);

        StartCoroutine(TitleLoadCoroutine());
    }

    private void Start()
    {
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
        loadingUI.SetActive(true);
        loadingSlider.value = 0.1f;

        asyncOp = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);

        if(asyncOp == null)
        {
            Logger.LogError($"{GetType()}::Missing asyncOperation");
            yield break;
        }

        asyncOp.allowSceneActivation = false;

        yield return new WaitForSeconds(0.3f);

        while (!asyncOp.isDone)
        {
            if(asyncOp.progress < 0.9f)
            {
                loadingSlider.value = asyncOp.progress < 0.1f ? 0.1f : asyncOp.progress;
                yield return null;
            }
            if(asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
                yield break;
            }
        }

        yield return null;
    }
}
