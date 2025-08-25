using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadingUIData : BaseUIData 
{
    public AsyncOperation AsyncOperation;
    public float fadeDuration;
}

/// <summary>
/// AsyncOperation의 allowSceneActivation = false를 보장하지 않음
/// </summary>
/// 
public class LoadingUI : BaseUI
{
    public Slider LoadingSlider;
    public TextMeshProUGUI TipText;

    private LoadingUIData LoadingUIData;

    private const float MIN_SLIDER_VALUE = 0.05f;
    private const float MAX_SLIDER_VALUE = 0.995f;

    public override void SetInfo(BaseUIData data)
    {
        data.onShow = StartLoading;
        base.SetInfo(data);

        LoadingUIData = data as LoadingUIData;
        if (LoadingUIData == null)
        {
            Logger.LogError($"{GetType()}:: Invalid LoadingUIData");
            return;
        }
        if(LoadingUIData.AsyncOperation == null)
        {
            Logger.LogError($"{GetType()}:: Missing AsyncOperation");
            return;
        }
        if(LoadingUIData.AsyncOperation.allowSceneActivation == true)
        {
            Logger.LogWarning($"{GetType()}:: AsyncOperation.allowSceneActivation is not false. Set AsyncOperation.allowSceneActivation false before create LoadingUI");
            LoadingUIData.AsyncOperation.allowSceneActivation = false;
            return;
        }

        LoadingSlider.value = MIN_SLIDER_VALUE;
    }

    private void StartLoading()
    {
        StartCoroutine(LoadingUICoroutine());
    }
    

    IEnumerator LoadingUICoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        while(!LoadingUIData.AsyncOperation.isDone)
        {
            if(LoadingUIData.AsyncOperation.progress < 0.9f)
            {
                float sliderValue = LoadingUIData.AsyncOperation.progress / 0.9f;
                if(LoadingSlider.value - sliderValue > 0.01f)
                {
                    LoadingSlider.value = Mathf.Lerp(LoadingSlider.value, sliderValue, Time.deltaTime * 5f);
                    yield return null;
                }
            }
            else
            {
                if(LoadingSlider.value < 0.99f)
                {
                    LoadingSlider.value = Mathf.Clamp(Mathf.Lerp(LoadingSlider.value, MAX_SLIDER_VALUE, Time.deltaTime * 5f), MIN_SLIDER_VALUE, MAX_SLIDER_VALUE);
                    yield return null;
                    continue;
                }
                else
                {
                    LoadingSlider.value = MAX_SLIDER_VALUE;
                    UIManager.Instance.Fade(Color.black, 0f, 1f, LoadingUIData.fadeDuration, 0f, true);
                    yield return new WaitForSeconds(LoadingUIData.fadeDuration);
                    LoadingUIData.AsyncOperation.allowSceneActivation = true;
                    break;
                }
            }
        }
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.3f, 0f, true);
        CloseUI();
    }

}
