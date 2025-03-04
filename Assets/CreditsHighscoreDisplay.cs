using UnityEngine;
using TMPro;

public class CreditsHighscoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    private const string HighscoreKey = "HighscorePlaytime";

    void Start()
    {
        float highscorePlaytime = PlayerPrefs.GetFloat(HighscoreKey, 0);
        highscoreText.text = "Best Playtime: " + FormatTime(highscorePlaytime);
    }

    string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
