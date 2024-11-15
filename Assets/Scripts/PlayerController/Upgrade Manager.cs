using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int upgradePoints = 5; // Số điểm nâng cấp có sẵn
    public float playerSpeed = 5f; // Tốc độ mặc định
    public int playerDamage = 10; // Damage mặc định
    public int playerMaxHealth = 100; // Máu tối đa mặc định

    // Hàm tăng tốc độ chạy
    public void IncreaseSpeed()
    {
        if (upgradePoints > 0)
        {
            playerSpeed += 1f; // Tăng tốc độ chạy lên 1 đơn vị
            upgradePoints--;
            Debug.Log("Tốc độ hiện tại: " + playerSpeed);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }

    // Hàm tăng damage súng
    public void IncreaseDamage()
    {
        if (upgradePoints > 0)
        {
            playerDamage += 5; // Tăng thêm 5 damage
            upgradePoints--;
            Debug.Log("Damage hiện tại: " + playerDamage);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }

    // Hàm tăng máu
    public void IncreaseHealth()
    {
        if (upgradePoints > 0)
        {
            playerMaxHealth += 10; // Tăng thêm 10 máu
            upgradePoints--;
            Debug.Log("Máu tối đa hiện tại: " + playerMaxHealth);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }
}
