using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;

    private const string ScoreKey = "PlayerScore";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Giữ đối tượng này qua các scene
            // Tải điểm từ PlayerPrefs, nếu chưa có thì mặc định là 0
            score = PlayerPrefs.GetInt(ScoreKey, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }
}
