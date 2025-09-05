using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    RawImage rawImage;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.texture = UIPlayerManager.Instance.GetUIPlayerRT(0);
    }

}
