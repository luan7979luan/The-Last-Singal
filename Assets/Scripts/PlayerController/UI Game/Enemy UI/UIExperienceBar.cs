using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import Text Mesh Pro

public class UIExperienceBar : MonoBehaviour
{
    public Slider experienceSlider;       // Tham chiếu đến Slider trong UI
    public TMP_Text experienceText;       // Hiển thị kinh nghiệm hiện tại và cần đạt
    public TMP_Text levelText;            // Hiển thị level của người chơi

    private PlayerExperience playerExperience;  // Tham chiếu đến script PlayerExperience

    void Start()
    {
        // Tìm đối tượng có script PlayerExperience trong scene
        playerExperience = FindObjectOfType<PlayerExperience>();
        if (playerExperience == null)
        {
            Debug.LogWarning("Không tìm thấy PlayerExperience trong scene!");
        }
    }

    void Update()
    {
        if (playerExperience != null)
        {
            // Tính phần trăm kinh nghiệm đạt được so với kinh nghiệm cần cho level tiếp theo
            float xpFraction = (float)playerExperience.currentExperience / playerExperience.experienceToNextLevel;
            experienceSlider.value = xpFraction;

            // Cập nhật TMP_Text hiển thị kinh nghiệm
            if (experienceText != null)
            {
                experienceText.text = $"{playerExperience.currentExperience} / {playerExperience.experienceToNextLevel}";
            }

            // Cập nhật hiển thị Level, định dạng 2 chữ số (ví dụ: 01, 02, 03)
            if (levelText != null)
            {
                levelText.text = playerExperience.level.ToString("00");
            }
        }
    }
}
