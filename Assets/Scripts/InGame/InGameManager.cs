public class InGameManager : Singleton<InGameManager>
{
    public bool isPaused { get; private set; } = false;

    protected override void Init()
    {
        m_IsDestroyOnLoad = false;
        base.Init();
    }

}
