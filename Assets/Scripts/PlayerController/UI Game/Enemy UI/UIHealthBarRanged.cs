using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Sử dụng TextMeshPro

public class UIHealthBarRanged : MonoBehaviour
{
    // Tham chiếu đến script Health của quái vật
    public Health monsterHealth;

    // Slider hiển thị lượng máu
    public Slider healthSlider;

    // TMP_Text hiển thị cấp của quái (Text Mesh Pro)
    public TMP_Text levelText;

    // Cấp của enemy (có thể đặt cố định nếu bạn không sử dụng logic tăng cấp)
    public int enemyLevel = 1;

    void Start()
    {
        // Nếu chưa gán, tự động tìm Health trên đối tượng cha
        if (monsterHealth == null)
        {
            monsterHealth = GetComponentInParent<Health>();
        }

        // Thiết lập giá trị ban đầu cho Slider
        if (healthSlider != null && monsterHealth != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = monsterHealth.maxHealth;
            healthSlider.value = monsterHealth.currentHealth;
        }

        // Cập nhật hiển thị cấp ban đầu (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }
    }

    void Update()
    {
        // Cập nhật giá trị của Slider theo lượng máu hiện tại
        if (monsterHealth != null && healthSlider != null)
        {
            healthSlider.value = monsterHealth.currentHealth;
        }
    }
}
