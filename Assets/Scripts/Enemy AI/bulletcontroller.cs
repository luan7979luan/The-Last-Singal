using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletcontroller : MonoBehaviour
{
   public float speed = 20f;
    public Rigidbody rb;

    void Start()
    {
        // Di chuyển đạn theo hướng forward
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Xử lý va chạm với các đối tượng (ví dụ: người chơi)
        if (other.CompareTag("Player"))
        {
            // Gây sát thương cho người chơi hoặc thực hiện các hành động khác
            Destroy(gameObject);  // Xoá viên đạn sau khi va chạm
        }
    }
}
