using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCamController : MonoBehaviour
{
  [SerializeField] private ARCameraManager _aRCamera;
  private Texture2D _texture;
  private Texture2D cameraTexture => _texture = _texture == null ? null : _texture;

  private void OnEnable()
  {
    if (_aRCamera != null)
    {
      _aRCamera.frameReceived += OnCameraFrameReceived;
    }
  }

  private void OnDisable()
  {
    if (_aRCamera != null)
    {
      _aRCamera.frameReceived -= OnCameraFrameReceived;
    }
  }

  private void GetTextureGPU(ARCameraFrameEventArgs eventArgs)
  {
    if (eventArgs.textures.Count > 0)
    {
      _texture = eventArgs.textures[eventArgs.textures.Count - 1];
    }
  }

  private unsafe void GetTextureCPU()
  {
    if (!_aRCamera.TryAcquireLatestCpuImage(out var cpuImage))
    {
      return;
    }
    if (_texture == null || _texture.width != cpuImage.width || _texture.height != cpuImage.height)
    {
      _texture = new Texture2D(cpuImage.width, cpuImage.height, TextureFormat.RGBA32, false);
    }

    var transformation = (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) ? XRCpuImage.Transformation.MirrorY : XRCpuImage.Transformation.MirrorX;
    var conversionParams = new XRCpuImage.ConversionParams(cpuImage, TextureFormat.RGBA32, transformation);
    var rawTextureData = _texture.GetRawTextureData<byte>();

    try
    {
      cpuImage.Convert(conversionParams, new System.IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
    }
    finally
    {
      cpuImage.Dispose();
    }
    _texture.Apply();
  }

  private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
  {
    GetTextureCPU();
    //GetTextureGPU(eventArgs);
  }

  public Texture2D GetCurrentTexture()
  {
    return cameraTexture;
  }
}
