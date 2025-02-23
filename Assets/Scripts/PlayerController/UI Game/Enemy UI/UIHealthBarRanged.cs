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

    // Các biến quản lý cấp và thời gian tăng cấp
    public int enemyLevel = 1;
    public float levelInterval = 120f; // 2 phút (120 giây)
    private float levelTimer = 0f;
    public int healthIncreasePerLevel = 50; // Mỗi cấp tăng thêm 50 máu

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

        // Cập nhật bộ đếm thời gian
        levelTimer += Time.deltaTime;
        if (levelTimer >= levelInterval)
        {
            LevelUp();
            levelTimer = 0f;
        }
    }

    // Hàm tăng cấp quái
    void LevelUp()
    {
        enemyLevel++;

        // Tăng máu tối đa của quái theo cấp (cộng thêm 50)
        if (monsterHealth != null)
        {
            monsterHealth.maxHealth += healthIncreasePerLevel;
            // Nếu muốn quái có máu đầy sau khi tăng cấp, bạn có thể bật dòng sau:
            // monsterHealth.currentHealth = monsterHealth.maxHealth;

            // Cập nhật lại giá trị max của Slider
            if (healthSlider != null)
            {
                healthSlider.maxValue = monsterHealth.maxHealth;
            }
        }

        // Cập nhật hiển thị cấp (định dạng 2 chữ số)
        if (levelText != null)
        {
            levelText.text = enemyLevel.ToString("00");
        }

        Debug.Log("Quái đã tăng cấp lên: " + enemyLevel + " với máu tối đa mới: " + monsterHealth.maxHealth);
    }
}
