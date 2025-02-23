using UnityEngine;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    public int currentExperience;
    public int experienceToNextLevel = 100;
    public int level = 1;

    // Số điểm nâng cấp có sẵn
    public int availableUpgradePoints = 0;
    
    // Tham chiếu đến TextMeshProUGUI để hiển thị số điểm nâng cấp (ví dụ: "Point Available: 01")
    public TextMeshProUGUI upgradePointsText;

    // Hàm cộng kinh nghiệm
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    // Hàm tăng cấp
    private void LevelUp()
    {
        level++;
        currentExperience -= experienceToNextLevel;
        experienceToNextLevel += 50;  // Ví dụ: tăng yêu cầu kinh nghiệm cho cấp tiếp theo
        availableUpgradePoints++;     // Cộng thêm 1 điểm nâng cấp mỗi khi lên cấp
        Debug.Log("Level Up! Cấp độ: " + level);
        UpdateUpgradePointsUI();
    }

    // Phương thức cập nhật UI hiển thị điểm nâng cấp (định dạng 2 chữ số: 01, 02, 03, ...)
    public void UpdateUpgradePointsUI()
    {
        if (upgradePointsText != null)
        {
            string formattedPoints = availableUpgradePoints.ToString("00");
            upgradePointsText.text = "Point Available: " + formattedPoints;
        }
    }
}
