using System;
using UnityEngine;

public class BaseUIData
{
    public Action onShow;
    public Action onClose;
}

public class BaseUI : MonoBehaviour
{
    public Animation m_UIOpenAnim;

    private Action m_OnShow;
    private Action m_OnClose;

    public virtual void Init(Transform anchor)
    {
        Logger.Log($"{GetType()}:: Init");

        m_OnShow = null;
        m_OnClose = null;

        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }

    public virtual void SetInfo(BaseUIData data)
    {
        Logger.Log($"{GetType()}:: SetInfo");
        m_OnShow = data.onShow;
        m_OnClose = data.onClose;
    }

    public virtual void ShowUI()
    {
        if(m_UIOpenAnim != null)
        {
            m_UIOpenAnim.Play();
        }
        m_OnShow?.Invoke();
        m_OnShow = null;
    }

    public virtual void CloseUI(bool isCloseAll = false)
    {
        if(!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;
        Logger.Log($"{GetType()}:: CloseUI");

        UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        //AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
