using System;
using UnityEngine;

public class BaseController : RobotController
{
  private GameObject _currentLink;

  private void ChangeColor(int index)
  {
    for (var i = 0; i < transform.childCount; i++)
    {
      try
      {
        transform.GetChild(i).GetComponent<Renderer>().material.color = color[index];
      }
      catch (Exception) { }
    }
  }

  protected override bool TryGetMovement(out Vector3 moveSpeed)
  {
    throw new NotImplementedException();
  }

  protected override bool TryGetRotation(out Vector3 rotationSpeed)
  {
    context = HandDetectorController.Instance.onRotationContext;
    if (context == "Left")
    {
      rotationSpeed = new Vector3(0, 25, 0);
      return true;
    }
    else if (context == "Right")
    {
      rotationSpeed = new Vector3(0, -25, 0);
      return true;
    }
    rotationSpeed = default;
    return false;
  }

  protected override bool TryGetScale(out Vector3 scaleSpeed)
  {
    context = HandDetectorController.Instance.onZoomContext;
    if (context == "Plus")
    {
      currentSize++;
      currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
      scaleSpeed = currentSize * 0.15f * Vector3.one;
      Debug.Log("MIA Plus " + scaleSpeed.ToString());
      return true;
    }
    else if (context == "Minus")
    {
      currentSize--;
      currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
      scaleSpeed = currentSize * 0.15f * Vector3.one;
      Debug.Log("MIA Minus " + scaleSpeed.ToString());
      return true;
    }
    Debug.Log("MIA sin zoom");
    scaleSpeed = default;
    return false;
  }

  private void Update()
  {
    if (HandDetectorController.Instance.robotSelect != null) { _currentLink = HandDetectorController.Instance.robotSelect; }
    if (_currentLink != null)
    {
      if (_currentLink.CompareTag(tag))
      {
        ChangeColor(1);
        if (transform.parent == null)
        {
          if (TryGetRotation(out var rotationSpeed))
          {
            transform.Rotate(rotationSpeed * Time.deltaTime);
          }
        }
        else
        {
          if (TryGetRotation(out var rotaionSpeed))
          {
            transform.parent.Rotate(rotaionSpeed * Time.deltaTime);
          }
        }
      }
      else
      {
        ChangeColor(0);
      }
    }
    else
    {
      ChangeColor(0);
    }
  }
}
