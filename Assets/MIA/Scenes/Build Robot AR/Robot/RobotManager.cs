using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
  private int _index = -1;
  public List<GameObject> links;

  public bool TryGetNextLink(out GameObject link)
  {
    if (_index < links.Count)
    {
      link = GetNextLink();
      return true;
    }
    link = default;
    return false;
  }

  private GameObject GetNextLink()
  {
    _index += 1;
    return links[_index];
  }
}
