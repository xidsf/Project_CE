using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CharacterUIRenderTexture : MonoBehaviour
{
    private int playerLayer;
    private int characterType;
    private GameObject instantiatedCharacter;
    
    [SerializeField] private Camera CharUICamPrefab;

    public RenderTexture RenderTexture {  get; private set; }

    private void Awake()
    {
        playerLayer  = LayerMask.NameToLayer("UIPlayer");
    }

    public void BindUIPlayer(CharacterType type)
    {
        characterType = (int)type;
        CreateRenderTexture();
        SetRTCamera();
        CreateCharacter(type);
    }

    private void CreateRenderTexture()
    {
        var desc = new RenderTextureDescriptor(512, 512)
        {
            graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
            depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt,
            msaaSamples = 1,
            useMipMap = false,
            autoGenerateMips = false,
            sRGB = (QualitySettings.activeColorSpace == ColorSpace.Linear),
        };

        RenderTexture = new RenderTexture(desc)
        {
            name = $"RT_{gameObject.name}_{characterType}",
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
            anisoLevel = 0
        };
    }

    private void SetRTCamera()
    {
        CharUICamPrefab.transform.localPosition = new Vector3(0, 0, -1);
        CharUICamPrefab.targetTexture = RenderTexture;
    }

    private void CreateCharacter(CharacterType type)
    {
        GameObject obj;
        if (ResourcesLoader.LoadPlayerPrefab(out obj, type))
        {
            instantiatedCharacter = Instantiate(obj, transform);
            instantiatedCharacter.transform.localPosition = new Vector3(0, -3.5f, 0);
            instantiatedCharacter.transform.localScale = Vector3.one * 10f;
            SetLayerRecursively(instantiatedCharacter, playerLayer);
        }
        else
        {
            Logger.LogError("Cannot Load CharacterPrefab");
        }
    }

    private void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        var t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }
}
