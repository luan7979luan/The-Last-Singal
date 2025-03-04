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

    // T?o hi?u ?ng ch?p s�ng 
    SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // S? kinh nghi?m th??ng cho player khi robot b? ti�u di?t
    public int experienceReward = 50;

    // Bi?n ki?m tra tr?ng th�i ch?t ?? ??m b?o h�nh ??ng Die ch? ???c g?i m?t l?n
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
        // L?y component NavMeshAgent n?u c�
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
        // N?u ?� ch?t r?i, kh�ng x? l� th�m
        if (isDead)
            return;

        currentHealth -= amount;
        // Khi s?c kh?e d??i ho?c b?ng 0, th?c hi?n h�nh ??ng ch?t ch? m?t l?n
        if (currentHealth <= 0.0f && !isDead)
        {
            isDead = true;

            // V� hi?u h�a controller c?a robot
            bossController.enabled = false;
            animator.SetTrigger("Die"); // K�ch ho?t animation ch?t

            // T?t NavMeshAgent ?? enemy kh�ng c�n di chuy?n n?a
            if (navAgent != null)
                navAgent.enabled = false;

            // C?ng kinh nghi?m cho player
            AddExperienceToPlayer();

            Destroy(gameObject, 3f); // H?y v?t th? sau 3 gi�y
        }
        // t?o blink khi b? ?�nh
        blinkTimer = blinkDuration;
    }

    // H�m t?o blink 
    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        _materialPropertyBlock.SetFloat("_blink", 0f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    // H�m c?ng kinh nghi?m cho player
    public void AddExperienceToPlayer()
    {
        // Gi? s? ??i t??ng player c� component PlayerExperience
        PlayerExperience playerExperience = FindObjectOfType<PlayerExperience>();
        if (playerExperience != null)
        {
            playerExperience.AddExperience(experienceReward);
        }
        else
        {
            Debug.LogWarning("Kh�ng t�m th?y PlayerExperience trong scene!");
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
