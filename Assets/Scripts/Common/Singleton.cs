using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //씬 전환 시 삭제할지 여부
    protected bool m_IsDestroyOnLoad = false;

    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindAnyObjectByType<T>();
                if (m_Instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    m_Instance = singletonObject.AddComponent<T>();
                }
            }
            return m_Instance;
        }
    }
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = (T)this;

            if (!m_IsDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else if(m_Instance != null && m_Instance.gameObject != gameObject)
        {
            Logger.Log($"{m_Instance.gameObject.name} is already exist. Destroy {gameObject.name}");
            Destroy(gameObject);
        }
    }

    //삭제 시 실행되는 함수
    protected virtual void OnDestroy()
    {
        Dispose();
    }

    //삭제 시 추가로 처리해 주어야할 작업을 여기서 처리
    protected virtual void Dispose()
    {
        m_Instance = null;
    }
}