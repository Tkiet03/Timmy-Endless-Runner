using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    [Header("Map Prefabs")]
    public GameObject[] segments;

    [Header("Player")]
    public Transform player;

    [Header("Spawn Settings")]
    public int segmentLength = 100;     // MỖI MAP CÁCH NHAU -100
    public int segmentsOnScreen = 5;     // Số map tồn tại cùng lúc
    public int startZ = 0;

    private float spawnZ;
    private List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        spawnZ = startZ;

        // Spawn map ban đầu
        for (int i = 0; i < segmentsOnScreen; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Khi player đi gần cuối map hiện tại → spawn map mới
        if (player.position.z < spawnZ + (segmentsOnScreen * -segmentLength))
        {
            SpawnSegment();
            DeleteOldSegment();
        }
    }

    void SpawnSegment()
    {
        int index = Random.Range(0, segments.Length);

        GameObject segment = Instantiate(
            segments[index],
            new Vector3(0, 0, spawnZ),
            Quaternion.identity
        );

        activeSegments.Add(segment);
        spawnZ -= segmentLength; // MỖI MAP LÙI -100
    }

    void DeleteOldSegment()
    {
        Destroy(activeSegments[0]);
        activeSegments.RemoveAt(0);
    }
}
