using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStackHelper : MonoBehaviour
{
    private void Start()
    {
        Camera mainCamera = Camera.main;
        var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
        mainCameraData.renderType = CameraRenderType.Base;

        Camera sceneUICamera = GetComponent<Camera>();
        var sceneCameraData = sceneUICamera.GetUniversalAdditionalCameraData();
        sceneCameraData.renderType = CameraRenderType.Overlay;
        mainCameraData.cameraStack.Add(sceneUICamera);

        Camera MainUICamera = UIManager.Instance.UICamera;
        var MainUICameraData = MainUICamera.GetUniversalAdditionalCameraData();
        MainUICameraData.renderType = CameraRenderType.Overlay;
        mainCameraData.cameraStack.Add(MainUICamera);
    }
}
