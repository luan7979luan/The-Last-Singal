using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;  // Thêm namespace này để sử dụng NavMeshAgent

public class Healthmelee : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    RobotControllerMelee robotControllerMelee;

    // Tạo hiệu ứng chớp sáng 
    SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Số kinh nghiệm thưởng cho player khi enemy bị tiêu diệt
    public int experienceReward = 50;

    // Tham chiếu đến NavMeshAgent nếu enemy sử dụng di chuyển theo NavMesh
    private NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        robotControllerMelee = GetComponent<RobotControllerMelee>();
        ragdoll = GetComponent<Ragdoll>();

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
        // Lấy component NavMeshAgent nếu có
        navAgent = GetComponent<NavMeshAgent>();

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            hitboxMelee hitBoxMelee = rigidBody.gameObject.AddComponent<hitboxMelee>();
            hitBoxMelee.healthmelee = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        // healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            animator.SetTrigger("Die");
            // Tắt controller của robot
            robotControllerMelee.enabled = false;

            // Tắt NavMeshAgent để enemy không còn di chuyển nữa
            if (navAgent != null)
                navAgent.enabled = false;

            // Cộng kinh nghiệm cho player
            AddExperienceToPlayer();

            Destroy(gameObject, 3f); // Hủy vật thể sau 3 giây
        }
        // tạo blink khi bị đánh
        StartCoroutine(MaterialBlink());
    }

    // Hàm tạo blink 
    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        _materialPropertyBlock.SetFloat("_blink", 0f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
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
        //blinkTimer -= Time.deltaTime;
        //float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        //float intensity = lerp * blinkIntensity + 1.0f;
        //skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
