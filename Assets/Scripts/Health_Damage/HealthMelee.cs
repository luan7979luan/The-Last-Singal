using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Healthmelee : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    RobotControllerMelee robotControllerMelee;

    // tạo hiệu ứng chớp sáng 
    SkinnedMeshRenderer skinnedMeshRenderer;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        robotControllerMelee = GetComponent<RobotControllerMelee>();
        ragdoll =  GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
            hitboxMelee hitBox = rigidBody.gameObject.AddComponent<hitboxMelee>();
            hitBox.healthmelee = this;
        }
    }
    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        //healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die();
            // turn off the robot controller
            robotControllerMelee.enabled = false;
        }

        blinkTimer = blinkDuration;

    }
    private void Die()
    {
        ragdoll.ActivateRagroll();
        //healthBar.gameObject.SetActive(false);
        
    }
    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }

}
