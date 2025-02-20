using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int currentExperience;
    public int experienceToNextLevel = 100;
    public int level = 1;

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
        Debug.Log("Level Up! Cấp độ: " + level);
    }
}
