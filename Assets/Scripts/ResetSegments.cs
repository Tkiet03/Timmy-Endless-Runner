#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ResetSegments : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Reset 8 Segments về chuẩn")]
    void ResetAllSegments()
    {
        var segs = GetComponentsInChildren<Transform>();
        for (int i = 1; i <= 8; i++) // bỏ qua parent
        {
            if (segs[i].name.Contains("Segment"))
            {
                segs[i].position = new Vector3(0, 0, (i - 8) * 100f); // -700, -600, ..., 0
                segs[i].rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        Debug.Log("Đã reset 8 segment về chuẩn!");
    }
#endif
}