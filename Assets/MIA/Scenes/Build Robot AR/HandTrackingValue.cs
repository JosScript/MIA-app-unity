using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingValue : MonoBehaviour
{
  public List<Detection> palmDetections = null;
  public List<NormalizedRect> handRectsFromPalmDetections = null;
  public List<NormalizedLandmarkList> handLandmarks = null;
  //public List<LandmarkList> handWorldLandmarks = null;
  public List<NormalizedRect> handRectsFromLandmarks = null;
  public List<ClassificationList> handedness = null;

  public static HandTrackingValue Instance;

  private void Awake()
  {
    if (Instance == null) { Instance = this; }
    else { Destroy(gameObject); }
  }

  public void SetValue(List<NormalizedLandmarkList> handLandmarks)
  {
    //this.palmDetections = palmDetections;
    //this.handRectsFromPalmDetections = handRectsFromPalmDetections;
    this.handLandmarks = handLandmarks;
    //this.handedness = handedness;
  }

  private void Update()
  {

  }
}
