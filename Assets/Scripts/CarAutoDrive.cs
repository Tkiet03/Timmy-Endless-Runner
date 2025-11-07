using UnityEngine;

public class CarAutoDrive : MonoBehaviour
{
    public Transform pointA;   // Kéo GameObject "A" vào đây
    public Transform pointB;   // Kéo GameObject "B" vào đây
    public float speed = 10f;  // Tốc độ xe

    void Start()
    {
        // 1. Spawn ngay tại A
        transform.position = pointA.position;
        transform.rotation = Quaternion.LookRotation(pointB.position - pointA.position);
    }

    void Update()
    {
        // 2. Di chuyển đến B
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, step);

        // 3. Dừng khi tới B (cách 0.1m)
        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            enabled = false; // tắt script
    }
}
