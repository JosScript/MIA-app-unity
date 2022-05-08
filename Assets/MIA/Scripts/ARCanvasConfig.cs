using UnityEngine;

public class ARCanvasConfig : MonoBehaviour
{
  private Canvas _canvas;

  private void Start()
  {
    _canvas = GetComponent<Canvas>();
    _canvas.renderMode = RenderMode.ScreenSpaceCamera;
    _canvas.worldCamera = Camera.main;
    _canvas.planeDistance = 100;
  }
}
