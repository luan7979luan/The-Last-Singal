using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarBoss : MonoBehaviour
{
    // Tham chiếu đến script HealthBoss của Boss
    public HealthBoss bossHealth;

    // Slider hiển thị lượng máu của Boss
    public Slider healthSlider;

    void Start()
    {
        // Nếu chưa gán, tự động tìm HealthBoss trên đối tượng cha
        if (bossHealth == null)
        {
            bossHealth = GetComponentInParent<HealthBoss>();
        }

        // Thiết lập giá trị ban đầu cho slider
        if (healthSlider != null && bossHealth != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = bossHealth.maxHealth;
            healthSlider.value = bossHealth.currentHealth;
        }
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại của Boss
        if (bossHealth != null && healthSlider != null)
        {
            healthSlider.value = bossHealth.currentHealth;
        }
    }
}
