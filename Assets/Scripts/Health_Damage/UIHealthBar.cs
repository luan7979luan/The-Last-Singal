using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
     public Slider healthSlider;   // Tham chiếu đến Slider trong Canvas
    public float maxHealth = 100f; // Máu tối đa của kẻ thù
    private float currentHealth;  // Máu hiện tại

    void Start()
    {
        currentHealth = maxHealth; // Gán máu ban đầu
        UpdateHealthUI();
    }

    // Gây sát thương cho kẻ thù
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không âm
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm chết nếu máu về 0
        }
    }

    // Cập nhật UI thanh máu
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // Hiển thị tỷ lệ phần trăm máu
        }
    }

    // Xử lý khi kẻ thù chết
    private void Die()
    {
        // Thêm logic cho việc kẻ thù chết (ví dụ: animation, hủy object)
        Destroy(gameObject); // Xóa kẻ thù khỏi Scene
    }
}
