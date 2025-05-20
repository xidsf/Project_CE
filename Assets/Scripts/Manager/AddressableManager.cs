using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    private GameObject instantiatedPlayer = null;
    public readonly Vector3 playerSpawnPos = new Vector3(0, -2.9f, 0);

    private AsyncOperationHandle loadedHandles;
    public readonly List<GameObject> loadedPrefabAssets = new List<GameObject>();
    public readonly List<SkillEffectSO> loadedSkillSOAssets = new List<SkillEffectSO>();

    //public readonly List<GameObject> instantiatedGameObject = new List<GameObject>();

    [SerializeField] private AssetLabelReference warriorAssetLabelReference;
    [SerializeField] private AssetLabelReference OtherLabelReference;


    private AssetLabelReference currentAssetLabel;

    private void Awake()
    {
        currentAssetLabel = warriorAssetLabelReference;
        LoadCurrentLabelAssets();
    }

    private void LoadCurrentLabelAssets()
    {
        Addressables.LoadAssetsAsync<Object>(currentAssetLabel, null).Completed += (obj) =>
        {
            loadedHandles = obj;
            foreach (var asset in obj.Result)
            {
                if (asset is GameObject prefab)
                {
                    if(instantiatedPlayer == null && prefab.GetComponent<Player>() != null)
                    {
                        instantiatedPlayer = Instantiate(prefab, playerSpawnPos, Quaternion.identity);
                    }
                    else
                    {
                        loadedPrefabAssets.Add(Instantiate(prefab));
                    }
                }
                else if (asset is SkillEffectSO skillSO)
                {
                    loadedSkillSOAssets.Add(skillSO);
                }
            }
        };
    }

    public void ReleaseAllLoadedAsset()
    {
        Destroy(instantiatedPlayer);
        instantiatedPlayer = null;

        loadedPrefabAssets.Clear();
        loadedSkillSOAssets.Clear();

        Addressables.Release(loadedHandles);
    }

    private void OnApplicationQuit()
    {
        ReleaseAllLoadedAsset();
    }
}
