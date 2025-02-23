using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Sử dụng TextMeshPro

public class UIHealthBarMelee : MonoBehaviour
{
    // Tham chiếu đến script Healthmelee của robot melee
    public Healthmelee robotHealth;

    // Slider hiển thị lượng máu
    public Slider healthSlider;

    // TMP_Text hiển thị cấp của robot melee
    public TMP_Text levelText;

    // Các biến quản lý cấp và thời gian tăng cấp
    public int enemyLevel = 1;
    public float levelInterval = 120f; // 2 phút (120 giây)
    private float levelTimer = 0f;
    public int healthIncreasePerLevel = 50; // Mỗi cấp tăng thêm 50 máu

    void Start()
    {
        // Nếu chưa gán, tự động tìm Healthmelee trên đối tượng cha
        if (robotHealth == null)
        {
            robotHealth = GetComponentInParent<Healthmelee>();
        }

        // Thiết lập giá trị ban đầu cho slider
        if (healthSlider != null && robotHealth != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = robotHealth.maxHealth;
            healthSlider.value = robotHealth.currentHealth;
        }

        // Cập nhật hiển thị cấp ban đầu (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại của robot melee
        if (robotHealth != null && healthSlider != null)
        {
            healthSlider.value = robotHealth.currentHealth;
        }

        // Cập nhật bộ đếm thời gian để tăng cấp
        levelTimer += Time.deltaTime;
        if (levelTimer >= levelInterval)
        {
            LevelUp();
            levelTimer = 0f;
        }
    }

    // Hàm tăng cấp robot melee
    void LevelUp()
    {
        enemyLevel++;

        // Tăng máu tối đa của robot melee theo cấp (cộng thêm 50)
        if (robotHealth != null)
        {
            robotHealth.maxHealth += healthIncreasePerLevel;
            // (Tùy chọn) Nếu muốn cho robot hồi đầy máu sau khi tăng cấp, bỏ comment dòng sau:
            // robotHealth.currentHealth = robotHealth.maxHealth;

            // Cập nhật lại giá trị max của slider
            if (healthSlider != null)
            {
                healthSlider.maxValue = robotHealth.maxHealth;
            }
        }

        // Cập nhật hiển thị cấp (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }

        Debug.Log("Robot melee đã tăng cấp lên: " + enemyLevel + " với máu tối đa mới: " + robotHealth.maxHealth);
    }
}
