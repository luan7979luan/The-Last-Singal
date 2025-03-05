using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsHighScore : MonoBehaviour
{
    // Gán đối tượng TextMeshProUGUI trong Inspector
    public TextMeshProUGUI scoreText;

    // Khóa lưu điểm trong PlayerPrefs
    private const string ScoreKey = "PlayerScore";

    void Start()
    {
        int score = 0;
        // Nếu ScoreManager tồn tại, lấy điểm từ đó
        if (ScoreManager.instance != null)
        {
            score = ScoreManager.instance.score;
        }
        else
        {
            // Nếu không, đọc từ PlayerPrefs
            score = PlayerPrefs.GetInt(ScoreKey, 0);
        }

        if (scoreText != null)
        {
            scoreText.text = "Total Score: " + score.ToString();
        }
    }
}
