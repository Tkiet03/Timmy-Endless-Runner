using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageControls : MonoBehaviour
{
    [Header("=== UI REFERENCES ===")]
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI stageNameText;

    [Header("=== MAP PREVIEWS ===")]
    public GameObject[] mapPreviews; // Kéo DesertRun và CountrySide vào đây

    [Header("=== MAP SETTINGS ===")]
    public string[] mapNames = { "Desert Run", "Country Side" };
    public string[] mapScenes = { "DesertRun", "CountrySide" }; // Tên scene thật trong Build Settings

    [Header("=== CAMERA SETTINGS ===")]
    public Transform stageCamera;                    // Kéo Main Camera vào
    public Vector3[] cameraPositions;                // Vị trí camera cho từng map (Size = số map)
    public Vector3[] cameraRotations;                // Góc nhìn (Euler) cho từng map
    [Tooltip("Thời gian camera di chuyển mượt (giây)")]
    public float cameraMoveDuration = 0.6f;

    private int currentMapIndex = 0;
    private Coroutine cameraMoveCoroutine;

    void Start()
    {
        // Bảo vệ null
        if (leftButton) leftButton.onClick.AddListener(PreviousMap);
        if (rightButton) rightButton.onClick.AddListener(NextMap);

        UpdateMapPreview(); // Hiển thị map đầu tiên + camera bay vào vị trí đúng
    }

    void Update()
    {
        // Test nhanh bằng phím
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousMap();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            NextMap();
    }

    public void PreviousMap()
    {
        currentMapIndex = (currentMapIndex - 1 + mapPreviews.Length) % mapPreviews.Length;
        UpdateMapPreview();
    }

    public void NextMap()
    {
        currentMapIndex = (currentMapIndex + 1) % mapPreviews.Length;
        UpdateMapPreview();
    }

    void UpdateMapPreview()
    {
        // === 1. Ẩn/hiện map preview ===
        for (int i = 0; i < mapPreviews.Length; i++)
        {
            if (mapPreviews[i] != null)
                mapPreviews[i].SetActive(i == currentMapIndex);
        }

        // === 2. Cập nhật tên map ===
        if (stageNameText != null)
            stageNameText.text = mapNames[currentMapIndex];

        // === 3. Di chuyển camera mượt mà đến vị trí mới ===
        if (stageCamera != null &&
            cameraPositions != null && cameraPositions.Length > currentMapIndex &&
            cameraRotations != null && cameraRotations.Length > currentMapIndex)
        {
            if (cameraMoveCoroutine != null)
                StopCoroutine(cameraMoveCoroutine);

            cameraMoveCoroutine = StartCoroutine(MoveCameraSmoothly(
                cameraPositions[currentMapIndex],
                cameraRotations[currentMapIndex]
            ));
        }
    }

    private IEnumerator MoveCameraSmoothly(Vector3 targetPosition, Vector3 targetEuler)
    {
        Vector3 startPos = stageCamera.position;
        Quaternion startRot = stageCamera.rotation;
        Quaternion endRot = Quaternion.Euler(targetEuler);

        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime; // dùng unscaled để mượt ngay cả khi Time.timeScale = 0
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);

            // Ease Out Quad cho cảm giác tự nhiên
            t = 1f - Mathf.Pow(1f - t, 3f);

            stageCamera.position = Vector3.Lerp(startPos, targetPosition, t);
            stageCamera.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        // Đảm bảo cuối cùng đúng 100%
        stageCamera.position = targetPosition;
        stageCamera.rotation = endRot;
    }

    // Nút Play – Load map đang chọn
    public void PressPlay()
    {
        if (string.IsNullOrEmpty(mapScenes[currentMapIndex]))
        {
            Debug.LogError("Map scene name is empty at index " + currentMapIndex);
            return;
        }

        SceneManager.LoadScene(mapScenes[currentMapIndex]);
    }

    // Hàm tiện ích (dùng trong Inspector hoặc debug)
    public int GetCurrentMapIndex() => currentMapIndex;
    public string GetCurrentMapName() => mapNames[currentMapIndex];
}