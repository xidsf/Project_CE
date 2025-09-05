using UnityEngine;

public class UIPlayerManager : Singleton<UIPlayerManager>
{
    [SerializeField] private GameObject CharacerUITemplate;
    private RenderTexture[] RenderTextures;

    protected override void Init()
    {
        base.Init();
        transform.position = new Vector3(0, -20, 0);
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        RenderTextures = new RenderTexture[(int)CharacterType.Count];

        for (int i = 0; i < (int)CharacterType.Count; i++)
        {
            var rtTemplate = Instantiate(CharacerUITemplate, transform);
            rtTemplate.transform.localPosition = new Vector3(i * 10, 0, 0);
            var CharacterUIRT = rtTemplate.GetComponent<CharacterUIRenderTexture>();
            if(CharacterUIRT != null)
            {
                CharacterUIRT.BindUIPlayer((CharacterType)i);
                RenderTextures[i] = CharacterUIRT.RenderTexture;
            }
        }
    }

    public RenderTexture GetUIPlayerRT(int index)
    {
        return RenderTextures[index];
    }

    public RenderTexture GetUIPlayerRT(CharacterType characterType)
    {
        return GetUIPlayerRT((int)characterType);
    }

}
