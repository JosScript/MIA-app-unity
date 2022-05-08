using UnityEngine;
using System.Collections.Generic;

public class JointController : MonoBehaviour
{
  [SerializeField] private GameObject _robotPrefab;
  private string _unionTag;

  // Start is called before the first frame update
  private void Start()
  {
    if (transform.parent.CompareTag("Base")) { _unionTag = "UBL"; }
    else if (transform.parent.CompareTag("Link01")) { _unionTag = "ULL"; }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!other.gameObject.CompareTag(_unionTag)) { return; }
    var robot = GameObject.FindGameObjectWithTag("Robot");
    if (robot == null)
    {
      robot = Instantiate(_robotPrefab);
    }
    AttachLinksBetween(transform.parent, other.gameObject.transform.parent, robot);
  }

  private void ActiveChild(GameObject parent)
  {
    for (var i = 0; i < parent.transform.childCount; i++)
    {
      parent.transform.GetChild(i).gameObject.SetActive(true);
    }
  }

  private void AttachLinksBetween(Transform oneLink, Transform anotherLink, GameObject robot)
  {
    var oldChilds = new List<Transform>();
    foreach (Transform child in robot.transform)
    {
      if (child != transform)
      {
        oldChilds.Add(child);
      }
    }
    robot.transform.DetachChildren();
    robot.transform.SetPositionAndRotation(oneLink.position, oneLink.rotation);
    oneLink.SetParent(robot.transform);
    foreach (var child in oldChilds)
    {
      child.SetParent(robot.transform);
    }
    anotherLink.SetPositionAndRotation(transform.position, robot.transform.rotation);
    anotherLink.SetParent(robot.transform);
    ActiveChild(anotherLink.gameObject);
  }
}
