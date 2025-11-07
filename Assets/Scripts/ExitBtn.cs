using UnityEngine;
using UnityEngine.UI; // Nếu dùng UI Button
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    // Gọi hàm này khi bấm nút EXIT
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");

        // Thoát game khi chạy bản build
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}