using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Sử dụng TextMeshPro

public class UIHealthBarNPC2 : MonoBehaviour
{
    // Tham chiếu đến script HealthNPC2 của NPC
    public HealthNPC2 npcHealth;

    // Slider hiển thị lượng máu của NPC
    public Slider healthSlider;

    // TMP_Text hiển thị cấp của NPC
    public TMP_Text levelText;

    // Các biến quản lý cấp và thời gian tăng cấp
    public int enemyLevel = 1;
    public float levelInterval = 120f; // 2 phút (120 giây)
    private float levelTimer = 0f;
    public int healthIncreasePerLevel = 50; // Mỗi cấp tăng thêm 50 máu

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

        // Cập nhật hiển thị cấp ban đầu (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }
    }

    void Update()
    {
        // Cập nhật giá trị của slider theo lượng máu hiện tại của NPC
        if (npcHealth != null && healthSlider != null)
        {
            healthSlider.value = npcHealth.currentHealth;
        }

        // Cập nhật bộ đếm thời gian để tăng cấp
        levelTimer += Time.deltaTime;
        if (levelTimer >= levelInterval)
        {
            LevelUp();
            levelTimer = 0f;
        }
    }

    // Hàm tăng cấp NPC
    void LevelUp()
    {
        enemyLevel++;

        // Tăng máu tối đa của NPC theo cấp (cộng thêm 50)
        if (npcHealth != null)
        {
            npcHealth.maxHealth += healthIncreasePerLevel;
            // (Tùy chọn) Nếu muốn cho NPC hồi đầy máu sau khi tăng cấp, bỏ comment dòng sau:
            // npcHealth.currentHealth = npcHealth.maxHealth;

            // Cập nhật lại giá trị max của slider
            if (healthSlider != null)
            {
                healthSlider.maxValue = npcHealth.maxHealth;
            }
        }

        // Cập nhật hiển thị cấp (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }

        Debug.Log("NPC đã tăng cấp lên: " + enemyLevel + " với máu tối đa mới: " + npcHealth.maxHealth);
    }
}
