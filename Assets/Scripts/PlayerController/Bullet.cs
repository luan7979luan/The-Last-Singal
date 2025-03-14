using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField] private Transform vfxHit;
    [SerializeField] private Transform vfxMiss; 

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            // Hit target
            Instantiate(vfxHit, transform.position, Quaternion.identity);
        }
        else
        {
            // Hit something else
            Instantiate(vfxMiss, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
