using System.Collections;
using UnityEngine;

public class SegmentsGenerated : MonoBehaviour
{
    public GameObject[] segment;
    [SerializeField] int zPos = -100; // Bắt đầu từ -100
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;

    void Start()
    {
        StartCoroutine(SegmentGen());
    }

    IEnumerator SegmentGen()
    {
        while (true) // Lặp vô hạn để tạo segment liên tục
        {
            if (!creatingSegment)
            {
                creatingSegment = true;
                segmentNum = Random.Range(0, 3);
                Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
                zPos += 50; // Tăng khoảng cách
                yield return new WaitForSeconds(3); // Chờ 3 giây
                creatingSegment = false;
            }
            yield return null; // Đợi frame tiếp theo
        }
    }
}