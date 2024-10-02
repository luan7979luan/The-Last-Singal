using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float radius;
    public GameObject prefab;
    public int max;
    public GameObject Player;
    public int count = 0;
    public Collider[] colliders;
    public float delay = 3;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        CheckPositionInsideCollider();
    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;
        if(timer > delay)
        {
        StartCoroutine (spawn());
        timer -= delay;
        }
    }
    IEnumerator spawn()
    {
        Debug.Log("1");
        if(count < max)
        {   
            Debug.Log("2");
            Vector3 random = new Vector3(UnityEngine.Random.Range(  -radius,  radius), UnityEngine.Random.Range( -radius, radius), UnityEngine.Random.Range(  -radius, radius));
            Boolean inside = false;
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].bounds.Contains(random))
                {
                inside = true;
                }
            }
            if(!inside)
            {
                Instantiate(prefab,  random , Quaternion.identity);
                count++;
            }
            
        }
        Debug.Log("5");
        yield return null;
    }

    private void CheckPositionInsideCollider()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);
    }

}