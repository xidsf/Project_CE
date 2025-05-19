using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableManager : MonoBehaviour
{
    
    List<GameObject> loadedAssets = new List<GameObject>();
    List<GameObject> instantiatedAsset = new List<GameObject>();

    [SerializeField] private AssetLabelReference warriorAssetLabelReference;

    private void Awake()
    {
        Addressables.LoadAssetsAsync<GameObject>(warriorAssetLabelReference, (gm) =>
        {
            loadedAssets.Add(gm);
            GameObject instantiatedObj = Instantiate(gm);
            instantiatedAsset.Add(instantiatedObj);
        });
    }


    public void Release()
    {
        foreach (var asset in loadedAssets)
        {
            Addressables.Release(asset);
        }
        loadedAssets.Clear();
    }

}
