using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Prefabs (Assign 7 segments here)")]
    public GameObject segmentMap01;
    public GameObject segmentMap02;
    public GameObject segmentMap03;
    public GameObject segmentMap04;
    public GameObject segmentMap05;
    public GameObject segmentMap06;
    public GameObject segmentMap07;

    [Header("Settings")]
    public float segmentLength = 100f;          // Khoảng cách giữa các segment (Z)
    public int initialSegmentCount = 8;         // Số segment spawn ban đầu
    public float spawnAheadDistance = 200f;     // Spawn mới khi player còn cách cuối < 200
    public float deleteBehindDistance = 300f;   // Xóa segment cũ khi cách player > 300

    private Transform playerTransform;
    private List<GameObject> activeSegments = new List<GameObject>();
    private float nextSpawnZ = 0f; // Bắt đầu từ Z=0

    // Array tự động từ 7 prefabs
    private GameObject[] segmentPrefabs;

    void Start()
    {
        // Tìm player "Timmy"
        playerTransform = GameObject.Find("Timmy").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Không tìm thấy 'Timmy'! Hãy tạo player tên Timmy.");
            return;
        }

        // Reset player về giữa segment đầu (Z=50)
        playerTransform.position = new Vector3(0f, 0f, 50f);

        // Tạo array prefabs từ 7 segmentMap
        segmentPrefabs = new GameObject[] { segmentMap01, segmentMap02, segmentMap03, segmentMap04, segmentMap05, segmentMap06, segmentMap07 };

        // XÓA TẤT CẢ SEGMENT CŨ TRONG SCENE (tag "Segment" để tránh chồng)
        GameObject[] existingSegments = GameObject.FindGameObjectsWithTag("Segment");
        foreach (GameObject seg in existingSegments)
        {
            DestroyImmediate(seg); // Destroy ngay lập tức trong Editor
        }
        activeSegments.Clear();

        // Spawn initial segments: đầu tiên tại (0,0,0) rot (0,90,0), tiếp theo -100, -200,...
        SpawnSegment(nextSpawnZ);
        nextSpawnZ -= segmentLength;
        for (int i = 1; i < initialSegmentCount; i++)
        {
            SpawnSegment(nextSpawnZ);
            nextSpawnZ -= segmentLength;
        }

        // KHÔNG dùng Coroutine nữa -> dùng Update cho mượt theo player speed
    }

    void Update()
    {
        // Tính khoảng cách đến điểm spawn tiếp theo (player chạy -Z)
        float distanceToEnd = playerTransform.position.z - nextSpawnZ;
        if (distanceToEnd < spawnAheadDistance)
        {
            SpawnSegment(nextSpawnZ);
            nextSpawnZ -= segmentLength;
        }

        // Xóa segments cũ PHÍA SAU player (Z > player.z + deleteBehindDistance)
        while (activeSegments.Count > 0 && activeSegments[0].transform.position.z > playerTransform.position.z + deleteBehindDistance)
        {
            Destroy(activeSegments[0]);
            activeSegments.RemoveAt(0);
        }
    }

    void SpawnSegment(float spawnZ)
    {
        // Random 1 trong 7 prefabs
        int randomIndex = Random.Range(0, segmentPrefabs.Length);
        GameObject prefab = segmentPrefabs[randomIndex];

        // VỊ TRÍ & ROTATION CHÍNH XÁC như scene: (0, 0, spawnZ), rot (0, 90, 0)
        Vector3 position = new Vector3(0f, 0f, spawnZ);
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

        // Instantiate & set tag để dễ xóa sau
        GameObject newSegment = Instantiate(prefab, position, rotation);
        newSegment.tag = "Segment"; // Tag để FindGameObjectsWithTag
        newSegment.name = "Segment_" + activeSegments.Count; // Tên gọn

        activeSegments.Add(newSegment);
    }
}