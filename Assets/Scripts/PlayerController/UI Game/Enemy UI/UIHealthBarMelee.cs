using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarMelee : MonoBehaviour
{
    // Tham chiếu đến script Healthmelee của robot melee
    public Healthmelee robotHealth;

    // Slider hiển thị lượng máu
    public Slider healthSlider;

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
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại của robot melee
        if (robotHealth != null && healthSlider != null)
        {
            healthSlider.value = robotHealth.currentHealth;
        }
    }
}