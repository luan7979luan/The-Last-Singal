using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTakeDamage: MonoBehaviour
{
    public PlayerHealth health;

    //Thanh slider dành cho máu của nhân vật
    public Slider healthSlider;

    private void Awake()
    {
       health = GetComponent<PlayerHealth>();
       healthSlider.maxValue = health.maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health.GetHealth();
    }


    //Player nhân sát thương
    public void TakeDamage(float damage, Vector3 position = new Vector3())
    {
        //Trừ máu
        health.TakeDamage(damage);

       
    }
}
