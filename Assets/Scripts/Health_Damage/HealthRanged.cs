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

    // Biến để kiểm tra trạng thái chết, tránh kích hoạt nhiều lần
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
        // Khi máu <= 0, xử lý hành động chết chỉ một lần
        if (currentHealth <= 0.0f && !isDead)
        {
            isDead = true; // Đánh dấu đã chết

            // Vô hiệu hóa controller và kích hoạt animation chết
            robotController.enabled = false;
            animator.SetTrigger("Die");

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

    /*
    // Nếu bạn muốn sử dụng hiệu ứng dissolve, có thể mở phần này lên
    private IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2);

        var dissolveTimeduration = 2f;
        var currentDissolveTime = 0f;
        var dissolveHeightStart = 30f;
        var dissolveHeightEnd = -10f;
        var dissolveHeight = dissolveHeightStart;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentDissolveTime < dissolveTimeduration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(dissolveHeightStart, dissolveHeightEnd, currentDissolveTime / dissolveTimeduration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
            skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }
    }
    */
}
