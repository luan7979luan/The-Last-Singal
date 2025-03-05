using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    Ragdoll ragdoll;
    public Slider healthSlider;
  
    public GameObject guns;
    killCam KillCamera;

    // UI Image dùng để hiển thị hiệu ứng damage (phải đảm bảo kéo thả đúng Image từ UI vào Inspector)
    public Image damageImage;
    // Tốc độ fade out của hiệu ứng
    public float flashSpeed = 5f;
    // Màu hiển thị khi nhận damage (màu đỏ với alpha cao)
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    // Thêm biến cho Game Over Panel
    public GameObject gameOverPanel;

    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        KillCamera = FindObjectOfType<killCam>();

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
        // Ẩn Game Over Panel khi bắt đầu game
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
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
       
        if (damageImage != null)
        {
            damageImage.color = flashColor;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpgradeHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;  
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Die()
{
    Debug.Log("Player đã chết!");
    ragdoll.ActivateRagdoll();
    Destroy(guns);
    KillCamera.EnableKillCam();

    // Hiển thị Game Over Panel
    if (gameOverPanel != null)
    {
        gameOverPanel.SetActive(true);
    }

    // Hiển thị con trỏ chuột
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

}
