using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Tên của scene chứa menu game, cần đảm bảo scene này đã được thêm vào Build Settings
    public string menuSceneName = "MainMenuScene";

    // Phương thức này sẽ được gọi khi button được click
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
