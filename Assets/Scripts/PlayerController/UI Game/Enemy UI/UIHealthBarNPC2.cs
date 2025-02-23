using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarNPC2 : MonoBehaviour
{
    // Tham chiếu đến script HealthNPC2 của NPC
    public HealthNPC2 npcHealth;

    // Slider hiển thị lượng máu của NPC
    public Slider healthSlider;

    void Start()
    {
        // Nếu chưa gán, tự động tìm HealthNPC2 trên đối tượng cha
        if (npcHealth == null)
        {
            npcHealth = GetComponentInParent<HealthNPC2>();
        }

        // Thiết lập giá trị ban đầu cho slider
        if (healthSlider != null && npcHealth != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = npcHealth.maxHealth;
            healthSlider.value = npcHealth.currentHealth;
        }
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại của NPC
        if (npcHealth != null && healthSlider != null)
        {
            healthSlider.value = npcHealth.currentHealth;
        }
    }
}
