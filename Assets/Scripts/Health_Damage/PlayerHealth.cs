using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider;
    
    // UI Image dùng để hiển thị hiệu ứng damage (phải đảm bảo kéo thả đúng Image từ UI vào Inspector)
    public Image damageImage;
    // Tốc độ fade out của hiệu ứng
    public float flashSpeed = 5f;
    // Màu hiển thị khi nhận damage (màu đỏ với alpha cao)
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        if (damageImage != null)
        {
            // Bắt đầu với màu trong suốt
            damageImage.color = Color.clear;
        }
    }

    void Update()
    {
        // Fade dần hiệu ứng damage về trong suốt
        if (damageImage != null)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player bị dame: " + damage + ", máu còn lại: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Khi bị dame, đặt màu cho damageImage để hiển thị hiệu ứng flash
        if (damageImage != null)
        {
            damageImage.color = flashColor;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Phương thức nâng cấp health: tăng maxHealth và currentHealth theo amount truyền vào
    public void UpgradeHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;  // Nếu bạn muốn tăng currentHealth theo amount
        // Nếu muốn currentHealth được khôi phục hoàn toàn, bạn có thể thay bằng: currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        Debug.Log("Health upgraded: maxHealth = " + maxHealth + ", currentHealth = " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player đã chết!");
        // Thêm các xử lý khi Player chết (chuyển cảnh, respawn, v.v.)
    }
}
