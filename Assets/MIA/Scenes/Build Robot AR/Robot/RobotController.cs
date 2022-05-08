using UnityEngine;

public abstract class RobotController : MonoBehaviour
{
  protected string context;
  protected readonly Color[] color = { Color.cyan, Color.blue };
  protected readonly float minSize = 0.5f;
  protected readonly float maxSize = 2f;
  protected float currentSize = 0.15f;

  protected abstract bool TryGetMovement(out Vector3 moveSpeed);
  protected abstract bool TryGetRotation(out Vector3 rotationSpeed);
  protected abstract bool TryGetScale(out Vector3 scaleSpeed);
}

