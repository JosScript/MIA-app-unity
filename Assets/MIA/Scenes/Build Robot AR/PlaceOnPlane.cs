using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
  [RequireComponent(typeof(ARRaycastManager))]
  public class PlaceOnPlane : MonoBehaviour
  {
    private static readonly List<ARRaycastHit> _Hits = new List<ARRaycastHit>();
    private ARRaycastManager _raycastManager;

    private bool _onClick = false;
    private RobotManager _robotArmManager;
    public GameObject spawnedObject { get; private set; }

    private void Awake()
    {
      _raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Start()
    {
      _robotArmManager = GameObject.Find("RobotManager").GetComponent<RobotManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
      var context = HandDetectorController.Instance.onClickContext.Split(' ');
      if (bool.Parse(context[0]))
      {
        touchPosition = new Vector2(float.Parse(context[1]), float.Parse(context[2]));
        if (HandDetectorController.Instance.robotSelect != null)
        {
          var obj = HandDetectorController.Instance.robotSelect;
          spawnedObject = obj.transform.parent != null ? obj.transform.parent.gameObject : obj;
        }
        return true;
      }
      touchPosition = default;
      return false;
    }

    private void Update()
    {
      if (TryGetTouchPosition(out var touchPosition))
      {
        if (_raycastManager.Raycast(touchPosition, _Hits, TrackableType.PlaneWithinPolygon))
        {
          var hitPose = _Hits[0].pose;
          if (!_onClick)
          {
            if (_robotArmManager.TryGetNextLink(out var link))
            {
              spawnedObject = Instantiate(link, hitPose.position, hitPose.rotation);
            }
            else
            {
              spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
            _onClick = true;
          }
        }
      }
      else { _onClick = false; }
    }
  }
}