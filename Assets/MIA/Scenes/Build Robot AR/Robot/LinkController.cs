using System;
using UnityEngine;

public class LinkController : RobotController
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
    context = HandDetectorController.Instance.onUpDownContext;
    if (context == "Up")
    {
      moveSpeed = new Vector3(0, 0.1f, 0);
      return true;
    }
    else if (context == "Down")
    {
      moveSpeed = new Vector3(0, -0.1f, 0);
      return true;
    }
    moveSpeed = default;
    return false;
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
      return true;
    }
    else if (context == "Minus")
    {
      currentSize--;
      currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
      scaleSpeed = currentSize * 0.15f * Vector3.one;
      return true;
    }
    scaleSpeed = default;
    return false;
  }

  private void GetLinkBehaviour()
  {
    if (_currentLink != null)
    {
      if (_currentLink.CompareTag(tag))
      {
        ChangeColor(1);
        if (TryGetMovement(out var moveSpeed))
        {
          if (transform.parent == null)
          {
            transform.Translate(moveSpeed * Time.deltaTime);
          }
          else
          {
            if (!GameObject.FindGameObjectWithTag("Base").transform.IsChildOf(transform.parent))
            {
              transform.parent.Translate(moveSpeed * Time.deltaTime);
            }
          }
        }
        if (TryGetRotation(out var rotationSpeed))
        {
          if (transform.parent == null)
          {
            transform.Rotate(rotationSpeed * Time.deltaTime);
          }
          else
          {
            if (!CompareTag("Link01"))
            {
              transform.rotation = GameObject.FindGameObjectWithTag("Link01").transform.rotation;
            }
          }
        }
        if (TryGetScale(out var scaleSpeed))
        {
          if (transform.parent == null)
          {
            transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleSpeed, 0.5f * Time.deltaTime);
          }
          else
          {
            transform.parent.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleSpeed, 0.5f * Time.deltaTime);
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

  // Update is called once per frame
  private void Update()
  {
    _currentLink = HandDetectorController.Instance.robotSelect;
    GetLinkBehaviour();
  }
}
