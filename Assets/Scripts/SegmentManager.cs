using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SegmentManager : MonoBehaviour
{
    [Header("=== THIẾT LẬP CHÍNH ===")]
    public Transform player;
    public float segmentLength = 100f;          // Độ dài mỗi segment (scene bạn = 100)

    [Header("=== TỰ ĐỘNG TÌM HOẶC GÁN THỦ CÔNG ===")]
    public bool autoFindSegments = true;
    public Transform[] manualSegments;          // Nếu tắt auto thì kéo 8 segment vào đây

    private Queue<Transform> segmentQueue = new Queue<Transform>();
    private float nextSpawnZ;                   // Z chính xác của segment tiếp theo sẽ spawn

    private void Awake() => InitPool();
    private void Start() => ResetSpawnPosition();
    private void Update() => SpawnIfNeeded();

    void InitPool()
    {
        Transform[] segments;

        if (autoFindSegments)
        {
            segments = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(go => go.CompareTag("Segment"))
                .Select(go => go.transform)
                .OrderBy(t => t.position.z)               // Sắp xếp từ nhỏ → lớn (Z âm → dương)
                .ToArray();
        }
        else
        {
            segments = manualSegments;
        }

        if (segments.Length == 0)
        {
            Debug.LogError("Không tìm thấy Segment nào! Tag = 'Segment' hoặc gán thủ công.");
            return;
        }

        foreach (var seg in segments)
            segmentQueue.Enqueue(seg);

        Debug.Log($"SegmentManager: Khởi tạo {segmentQueue.Count} segment thành công!");
    }

    void ResetSpawnPosition()
    {
        // Tính Z của segment đầu tiên (xa nhất phía trước player)
        Transform firstSeg = segmentQueue.ToArray()[0];
        nextSpawnZ = firstSeg.position.z - segmentLength;

        Debug.Log($"Bắt đầu spawn từ Z = {nextSpawnZ}");
    }

    void SpawnIfNeeded()
    {
        if (player == null || segmentQueue.Count == 0) return;

        // Chỉ spawn khi player tiến gần đến vị trí cần segment mới
        while (player.position.z < nextSpawnZ + segmentLength * 2f)   // buffer 2 segment phía trước
        {
            Transform segmentToSpawn = segmentQueue.Dequeue();

            // Đặt đúng vị trí chính xác, không lệch 1 ly nào
            segmentToSpawn.position = new Vector3(0f, 0f, nextSpawnZ);

            // Muốn random hướng nhẹ cho đẹp (bạn có thể bật dòng dưới
            // segmentToSpawn.rotation = Quaternion.Euler(0f, 90f + Random.Range(-10f, 10f), 0f);

            // Đưa lại vào cuối hàng đợi
            segmentQueue.Enqueue(segmentToSpawn);

            // Cập nhật vị trí spawn kế tiếp – ĐẢM BẢO LUÔN ĐỀU 100%
            nextSpawnZ -= segmentLength;

            // Debug (bỏ sau khi ổn)
            // Debug.Log($"Spawn segment tại Z = {nextSpawnZ + segmentLength}");
        }
    }

    // Gọi hàm này khi Game Over → Restart để reset lại map
    public void ResetMap()
    {
        if (segmentQueue.Count == 0) return;

        float currentZ = player.position.z + 300f; // bắt đầu lại phía trước player 300 unit
        foreach (var seg in segmentQueue)
        {
            seg.position = new Vector3(0f, 0f, currentZ);
            currentZ += segmentLength;
        }
        nextSpawnZ = currentZ;
    }
}