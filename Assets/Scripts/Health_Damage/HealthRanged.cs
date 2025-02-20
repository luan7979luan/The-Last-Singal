using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    RobotController robotController;

    // Tạo hiệu ứng chớp sáng 
    SkinnedMeshRenderer skinnedMeshRenderer;
    public MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Số kinh nghiệm thưởng cho player khi robot bị tiêu diệt
    public int experienceReward = 50;

    // Biến kiểm tra trạng thái chết để đảm bảo hành động Die chỉ được gọi một lần
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        robotController = GetComponent<RobotController>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        // Nếu đã chết rồi, không xử lý thêm
        if (isDead)
            return;

        currentHealth -= amount;
        // Khi sức khỏe dưới hoặc bằng 0, thực hiện hành động chết chỉ một lần
        if (currentHealth <= 0.0f && !isDead)
        {
            isDead = true;

            // Vô hiệu hóa controller của robot
            robotController.enabled = false;
            animator.SetTrigger("Die"); // Kích hoạt animation chết

            // Cộng kinh nghiệm cho player
            AddExperienceToPlayer();

            Destroy(gameObject, 3f); // Hủy vật thể sau 3 giây
        }

        blinkTimer = blinkDuration;
    }

    // Hàm cộng kinh nghiệm cho player
    public void AddExperienceToPlayer()
    {
        // Giả sử đối tượng player có component PlayerExperience
        PlayerExperience playerExperience = FindObjectOfType<PlayerExperience>();
        if (playerExperience != null)
        {
            playerExperience.AddExperience(experienceReward);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy PlayerExperience trong scene!");
        }
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
