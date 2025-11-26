using UnityEngine;

public class SprintFOVKick : MonoBehaviour
{
    public Camera mainCamera;
    public float normalFOV = 60f;       // FOV khi đứng yên / chạy bình thường
    public float sprintFOV = 75f;       // FOV khi giữ Shift (tăng mạnh cho cảm giác tốc độ)
    public float changeSpeed = 8f;      // Tốc độ chuyển mượt mà

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        // Giữ Left Shift → FOV tăng mạnh
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, sprintFOV, Time.deltaTime * changeSpeed);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalFOV, Time.deltaTime * changeSpeed);
        }
    }
}