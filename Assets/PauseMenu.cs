using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Gán đối tượng UI của pause menu trong Inspector
    public GameObject pauseMenuUI;
    // Tên của scene menu chính (Main Menu)
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    void Update()
    {
        // Nhấn phím Escape để bật/tắt pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

   public void PauseGame()
{
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f; // Dừng thời gian game
    isPaused = true;
    
    // Hiển thị con trỏ và mở khóa nó
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
}

public void ResumeGame()
{
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f; // Khôi phục thời gian game
    isPaused = false;
    
    // Ẩn con trỏ và khóa lại nếu cần
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
}


    // Phương thức restart game
    public void RestartGame()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian đang chạy khi tải lại scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Phương thức lưu dữ liệu (nếu cần) và chuyển về scene menu chính
    public void QuitAndSave()
{
    // Tìm đối tượng chứa PlaytimeCounter và gọi lưu highscore
    PlaytimeCounter playtimeCounter = FindObjectOfType<PlaytimeCounter>();
    if (playtimeCounter != null)
    {
        playtimeCounter.SaveHighscore();
    }
    
    Time.timeScale = 1f; // Đảm bảo thời gian game đang chạy khi chuyển scene
    SceneManager.LoadScene(mainMenuSceneName);
}

}
