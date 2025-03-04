using UnityEngine;
using TMPro;

public class PlaytimeCounter : MonoBehaviour
{
    // Nếu bạn muốn hiển thị thời gian chơi trong lúc game (không bắt buộc)
    public TextMeshProUGUI playtimeText;

    private float playtime = 0f; // Tổng số giây đã chơi
    private float highscorePlaytime = 0f;
    private const string HighscoreKey = "HighscorePlaytime";

    void Start()
    {
        // Lấy highscore đã lưu (nếu có)
        highscorePlaytime = PlayerPrefs.GetFloat(HighscoreKey, 0);
    }

    void Update()
    {
        playtime += Time.deltaTime;
        if (playtimeText != null)
            playtimeText.text = FormatTime(playtime);
    }

    string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    // Gọi phương thức này khi game kết thúc hoặc khi chuyển sang scene khác (nếu không dùng OnApplicationQuit)
    public void SaveHighscore()
    {
        if (playtime > highscorePlaytime)
        {
            highscorePlaytime = playtime;
            PlayerPrefs.SetFloat(HighscoreKey, highscorePlaytime);
            PlayerPrefs.Save();
        }
    }

    void OnApplicationQuit()
    {
        SaveHighscore();
    }
}
