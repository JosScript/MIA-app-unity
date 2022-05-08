using System;
using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandDetectorController : MonoBehaviour
{
  public static HandDetectorController Instance;

  public List<NormalizedLandmarkList> handLandmarks = null;
  public List<ClassificationList> handedness = null;

  public GameObject pointReference { get; private set; }

  public GameObject robotSelect { get; private set; }
  public string onClickContext { get; private set; }
  public string onUpDownContext { get; private set; }
  public string onRotationContext { get; private set; }
  public string onZoomContext { get; private set; }

  [SerializeField] private LayerMask _layerMask;

  [SerializeField] private Utils _utils;

  private void Awake()
  {
    if (Instance == null) { Instance = this; }
    else { Destroy(gameObject); }
  }

  private void Start()
  {
    onClickContext = "false";
    onRotationContext = "";
    onUpDownContext = "";
    onZoomContext = "";
  }

  public void SetValue(List<NormalizedLandmarkList> handLandmarks, List<ClassificationList> handedness)
  {
    this.handLandmarks = handLandmarks;
    this.handedness = handedness;
  }

  private List<int> FingersUp()
  {
    var _fingers = new List<int>(5);
    try
    {
      if (handedness != null && handedness.Count > 0)
      {
        if (handedness[0].Classification[0].Label == "Left")
        {
          if (handLandmarks[0].Landmark[4].X < handLandmarks[0].Landmark[3].X) { _fingers.Add(1); }
          else { _fingers.Add(0); }
        }
        else
        {
          if (handLandmarks[0].Landmark[4].X > handLandmarks[0].Landmark[3].X) { _fingers.Add(1); }
          else { _fingers.Add(0); }
        }
        for (var j = 8; j <= 20; j += 4)
        {
          if (handLandmarks[0].Landmark[j].Y > handLandmarks[0].Landmark[j - 2].Y) { _fingers.Add(0); }
          else { _fingers.Add(1); }
        }
      }
      Debug.Log(_fingers[0].ToString() + " " + _fingers[1].ToString() + " " + _fingers[2].ToString() + " " + _fingers[3].ToString() + " " + _fingers[4].ToString());
    }
    catch (Exception)
    {
      return null;
    }

    return _fingers;
  }

  private void OnClick()
  {
    var distance = _utils.Distance(new Vector2(handLandmarks[0].Landmark[8].X, handLandmarks[0].Landmark[8].Y), new Vector2(handLandmarks[0].Landmark[12].X, handLandmarks[0].Landmark[12].Y));
    Debug.Log("MIA ditance " + distance.ToString());
    if (distance < 0.05)
    {
      Vector2 clickPos = _utils.GetWorldPostion((new Vector2(handLandmarks[0].Landmark[8].X, handLandmarks[0].Landmark[8].Y) + new Vector2(handLandmarks[0].Landmark[12].X, handLandmarks[0].Landmark[12].Y)) / 2);
      onClickContext = "true" + " " + clickPos.x.ToString() + " " + clickPos.y.ToString();
    }
    else
    {
      onClickContext = "false";
    }
  }

  private void Zoom()
  {
    var p1 = new Vector2(handLandmarks[0].Landmark[8].X, handLandmarks[0].Landmark[8].Y);
    var p2 = new Vector2(handLandmarks[0].Landmark[4].X, handLandmarks[0].Landmark[4].Y);
    p1 = _utils.GetWorldPostion(p1);
    p2 = _utils.GetWorldPostion(p2);
    var distance = _utils.Distance(p1, p2);
    onZoomContext = distance > 200 ? "Plus" : "Minus";
  }

  private void Rotation()
  {
    if (handLandmarks[0].Landmark[8].X > handLandmarks[0].Landmark[6].X) { onRotationContext = "Left"; }
    else if (handLandmarks[0].Landmark[8].X < handLandmarks[0].Landmark[6].X) { onRotationContext = "Right"; }
  }

  private void MoveUpDown(int isUp)
  {
    if (isUp == 1) { onUpDownContext = "Up"; }
    else if (isUp == 0) { onUpDownContext = "Down"; }
  }

  /*  void FollowHand()
    {
        Vector3 reference = Camera.main.ScreenToWorldPoint(utils.GetWorldPostion(new Vector2(HandLandmarkLists[0].Landmark[9].X, HandLandmarkLists[0].Landmark[9].Y)));
        if (pointReference == null)
        {
            pointReference = Instantiate(m_PlacedPrefab, reference, Quaternion.identity);
            pointReference.transform.localScale = 0.015f * Vector3.one;
        }
        else
        {
            pointReference.transform.position = reference;
        }
        GameObject.FindGameObjectWithTag("OnRotation").GetComponent<Text>().text = "Hand";
    }*/

  private void TargetObject()
  {
    var p1 = _utils.GetWorldPostion(new Vector2(handLandmarks[0].Landmark[8].X, handLandmarks[0].Landmark[8].Y));
    var ray = Camera.main.ScreenPointToRay(new Vector2(p1.x, p1.y));
    if (Physics.Raycast(ray, out var hit, float.PositiveInfinity, _layerMask))
    {
      switch (hit.collider.tag)
      {
        case "Base":
          robotSelect = GameObject.FindGameObjectWithTag("Base");
          break;
        case "Link01":
          robotSelect = GameObject.FindGameObjectWithTag("Link01");
          break;
        case "Link02":
          robotSelect = GameObject.FindGameObjectWithTag("Link02");
          break;
        case "Link03":
          robotSelect = GameObject.FindGameObjectWithTag("Link03");
          break;
        default:
          break;
      }
      if (robotSelect != null)
      {
        Debug.Log(robotSelect.name + " " + robotSelect.transform.position.ToString());
      }
    }
  }

  /*void TweezersController()
  {
      Vector3 p1 = GetWorldPostion(new Vector2(HandLandmarkLists[0].Landmark[4].X, HandLandmarkLists[0].Landmark[4].Y));
      Vector3 p2 = GetWorldPostion(new Vector2(HandLandmarkLists[0].Landmark[8].X, HandLandmarkLists[0].Landmark[8].Y));
      float distance = Distance(p1, p2);
      GameObject.FindGameObjectWithTag("Debug1").GetComponent<Text>().text = distance.ToString();
  }*/

  private void Update()
  {
    var _fingers = FingersUp();
    if (_fingers != null)
    {
      if (_fingers[0] == 0 && _fingers[1] == 1 && _fingers[2] == 1 && _fingers[3] == 0 && _fingers[4] == 0)
      {
        OnClick();
      }
      else { onClickContext = "false"; }
      //---------------------------------------------------------------------------------------------------------------
      if (_fingers[0] == 1 && _fingers[1] == 1 && _fingers[2] == 0 && _fingers[3] == 0 && _fingers[4] == 0)
      {
        MoveUpDown(1);
      }
      else if (_fingers[0] == 1 && _fingers[1] == 0 && _fingers[2] == 0 && _fingers[3] == 0 && _fingers[4] == 0)
      {
        MoveUpDown(0);
      }
      else { onUpDownContext = ""; }
      //----------------------------------------------------------------------------------------------------------------
      if (_fingers[0] == 1 && _fingers[1] == 1 && _fingers[2] == 1 && _fingers[3] == 0 && _fingers[4] == 0)
      {
        Rotation();
      }
      else { onRotationContext = ""; }
      //----------------------------------------------------------------------------------------------------------------  
      if (_fingers[0] == 1 && _fingers[1] == 0 && _fingers[2] == 1 && _fingers[3] == 1 && _fingers[4] == 1)
      {
        Zoom();
      }
      else { onZoomContext = ""; }
      //----------------------------------------------------------------------------------------------------------------  
      if (_fingers[0] == 1 && _fingers[1] == 0 && _fingers[2] == 0 && _fingers[3] == 0 && _fingers[4] == 1)
      {
        robotSelect = null;
      }
    }
  }

  private void FixedUpdate()
  {
    var _fingers = FingersUp();
    if (_fingers != null)
    {
      if (_fingers[0] == 0 && _fingers[1] == 1 && _fingers[2] == 0 && _fingers[3] == 0 && _fingers[4] == 0)
      {
        TargetObject();
      }
    }
  }
}