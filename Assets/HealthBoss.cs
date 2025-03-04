using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HealthBoss : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    BossController bossController;

    // T?o hi?u ?ng ch?p sáng 
    SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // S? kinh nghi?m th??ng cho player khi robot b? tiêu di?t
    public int experienceReward = 50;

    // Bi?n ki?m tra tr?ng thái ch?t ?? ??m b?o hành ??ng Die ch? ???c g?i m?t l?n
    private bool isDead = false;

    // Tham chi?u ??n NavMeshAgent n?u enemy s? d?ng di chuy?n theo NavMesh
    private NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        bossController = GetComponent<BossController>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
        // L?y component NavMeshAgent n?u có
        navAgent = GetComponent<NavMeshAgent>();

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBoxBoss hitBoxBoss = rigidBody.gameObject.AddComponent<HitBoxBoss>();
            hitBoxBoss.healthBoss = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        // N?u ?ã ch?t r?i, không x? lý thêm
        if (isDead)
            return;

        currentHealth -= amount;
        // Khi s?c kh?e d??i ho?c b?ng 0, th?c hi?n hành ??ng ch?t ch? m?t l?n
        if (currentHealth <= 0.0f && !isDead)
        {
            isDead = true;

            // Vô hi?u hóa controller c?a robot
            bossController.enabled = false;
            animator.SetTrigger("Die"); // Kích ho?t animation ch?t

            // T?t NavMeshAgent ?? enemy không còn di chuy?n n?a
            if (navAgent != null)
                navAgent.enabled = false;

            // C?ng kinh nghi?m cho player
            AddExperienceToPlayer();

            Destroy(gameObject, 3f); // H?y v?t th? sau 3 giây
        }
        // t?o blink khi b? ?ánh
        blinkTimer = blinkDuration;
    }

    // Hàm t?o blink 
    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        _materialPropertyBlock.SetFloat("_blink", 0f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    // Hàm c?ng kinh nghi?m cho player
    public void AddExperienceToPlayer()
    {
        // Gi? s? ??i t??ng player có component PlayerExperience
        PlayerExperience playerExperience = FindObjectOfType<PlayerExperience>();
        if (playerExperience != null)
        {
            playerExperience.AddExperience(experienceReward);
        }
        else
        {
            Debug.LogWarning("Không tìm th?y PlayerExperience trong scene!");
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
