using UnityEngine;
using UnityEngine.UI;
public class UIHealthBarRanged : MonoBehaviour
{
    // Tham chiếu đến script Health của quái vật
    public Health monsterHealth;

    // Slider hiển thị lượng máu
    public Slider healthSlider;

    void Start()
    {
        // Nếu chưa gán, tự động tìm Health trên parent
        if (monsterHealth == null)
        {
            monsterHealth = GetComponentInParent<Health>();
        }

        // Thiết lập giá trị ban đầu cho slider
        if (healthSlider != null && monsterHealth != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = monsterHealth.maxHealth;
            healthSlider.value = monsterHealth.currentHealth;
        }
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại
        if (monsterHealth != null && healthSlider != null)
        {
            healthSlider.value = monsterHealth.currentHealth;
        }
    }
}
