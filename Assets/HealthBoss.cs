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

        SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    
    public int experienceReward = 50;

       private bool isDead = false;

    
    private NavMeshAgent navAgent;

    
    void Start()
    {
        bossController = GetComponent<BossController>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
       
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
        
        if (isDead)
            return;

        currentHealth -= amount;
        
        if (currentHealth <= 0.0f && !isDead)
        {
            isDead = true;

           
            bossController.enabled = false;
            animator.SetTrigger("Die"); 
           
            if (navAgent != null)
                navAgent.enabled = false;

          
            AddExperienceToPlayer();

            Destroy(gameObject, 3f); 
        }
        
        blinkTimer = blinkDuration;
    }

   
    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        _materialPropertyBlock.SetFloat("_blink", 0f);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    
    public void AddExperienceToPlayer()
    {
        
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
