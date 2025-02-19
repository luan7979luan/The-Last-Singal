using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int currentExperience;
    public int level;
    public int experienceToNextLevel = 100;

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        Debug.Log("Đã cộng " + amount + " kinh nghiệm. Tổng kinh nghiệm: " + currentExperience);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.5f);
            Debug.Log("Thăng cấp! Cấp hiện tại: " + level);
        }
    }
}
