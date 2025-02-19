using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExperienceBar : MonoBehaviour
{
    // Tham chiếu đến script PlayerExperience của player
    public PlayerExperience playerExperience;

    // Thành phần UI hiển thị kinh nghiệm sử dụng TextMeshProUGUI
    public TextMeshProUGUI experienceText;
    
    // Thanh kinh nghiệm (Slider của Unity UI)
    public Slider experienceSlider;

    void Start()
    {
        // Nếu chưa gán, tự động tìm PlayerExperience trong scene
        if (playerExperience == null)
        {
            playerExperience = FindObjectOfType<PlayerExperience>();
        }

        // Thiết lập giá trị cho Slider nếu có
        if (experienceSlider != null && playerExperience != null)
        {
            experienceSlider.minValue = 0;
            experienceSlider.maxValue = playerExperience.experienceToNextLevel;
        }
    }

    void Update()
    {
        if (playerExperience != null)
        {
            // Cập nhật nội dung Text hiển thị kinh nghiệm theo định dạng "0/100"
            if (experienceText != null)
            {
                experienceText.text = playerExperience.currentExperience + "/" + playerExperience.experienceToNextLevel;
            }

            // Cập nhật giá trị và maxValue của Slider
            if (experienceSlider != null)
            {
                experienceSlider.maxValue = playerExperience.experienceToNextLevel;
                experienceSlider.value = playerExperience.currentExperience;
            }
        }
    }
}
