using UnityEngine;

public class Utils : MonoBehaviour
{
    public float Distance(Vector3 p1, Vector3 p2)
    {
        float x1 = p1.x, y1 = p1.y, x2 = p2.x, y2 = p2.y, z1 = p1.z, z2 = p2.z;
        float distance = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));

        return distance;
    }

    public Vector3 GetWorldPostion(Vector2 vector)
    {
        Vector3 vec = new Vector3(Screen.width * (1 - vector.x), Screen.height * (1 - vector.y), 0.5f);
        return vec;
    }
}
