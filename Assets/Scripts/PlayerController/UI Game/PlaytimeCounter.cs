using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nếu sử dụng TextMeshPro

public class PlaytimeCounter : MonoBehaviour
{
    public Text uiText; // Sử dụng UI Text
    public TextMeshProUGUI tmpText; // Sử dụng TextMeshPro (nếu có)

    private float playtime = 0f; // Tổng số giây đã chơi

    void Update()
    {
        // Tăng thời gian chơi
        playtime += Time.deltaTime;

        // Cập nhật UI
        UpdatePlaytimeDisplay(playtime);
    }

    void UpdatePlaytimeDisplay(float time)
    {
        // Chuyển đổi thời gian thành giờ, phút và giây
        int hours = Mathf.FloorToInt(time / 3600); // 1 giờ = 3600 giây
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // Định dạng thời gian kiểu giờ:phút:giây
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        // Cập nhật UI
        if (uiText != null)
            uiText.text = formattedTime;
        if (tmpText != null)
            tmpText.text = formattedTime;
    }
}
