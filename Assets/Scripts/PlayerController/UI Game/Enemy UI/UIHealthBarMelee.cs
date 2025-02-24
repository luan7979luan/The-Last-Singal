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

    // Cấp của enemy, được cập nhật từ Director
    public int enemyLevel = 1;

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
    }

    // Phương thức cập nhật cấp của enemy, được gọi từ Director sau khi spawn hoặc khi thay đổi scaling
    public void SetEnemyLevel(int level)
    {
        enemyLevel = level;
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }
    }
}
