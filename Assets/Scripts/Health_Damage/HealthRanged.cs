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

    // tạo hiệu ứng chớp sáng 
    SkinnedMeshRenderer skinnedMeshRenderer;
    public MaterialPropertyBlock _materialPropertyBlock;

    public Animator animator;

    UIHealthBar healthBar;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        robotController = GetComponent<RobotController>();
        ragdoll =  GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }
    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        //healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            // turn off the robot controller
            robotController.enabled = false;
            animator.SetTrigger("Die"); // chet
                                        //StartCoroutine(MaterialDissolve());// tao hieu ung bien mat

            Destroy(gameObject, 3f); // huy vat the
        }

        blinkTimer = blinkDuration;

    }
    //private void Die()
    //{
    //    animator.SetTrigger("Die");
    //    //ragdoll.ActivateRagroll();
    //    //healthBar.gameObject.SetActive(false);

    //}
    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }

    //private IEnumerator MaterialDissolve()
    //{
    //    yield return new WaitForSeconds(2);

    //    var dissolveTimeduration = 2f;
    //    var currentDissolveTime = 0f;
    //    var dissolveHeightStart = 30f;
    //    var dissolveHeightEnd = -10f;
    //    var dissolveHeight = dissolveHeightStart;

    //    _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
    //    skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

    //    while( currentDissolveTime < dissolveTimeduration )
    //    {
    //        currentDissolveTime += Time.deltaTime;
    //        dissolveHeight = Mathf.Lerp(dissolveHeightStart, dissolveHeightEnd, currentDissolveTime / dissolveTimeduration);
    //        _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
    //        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    //        yield return null;
    //    }
    //}

}
