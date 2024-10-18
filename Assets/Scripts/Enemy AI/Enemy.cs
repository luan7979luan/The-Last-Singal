using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;   // Điểm bắn đạn ra
    public GameObject bulletPrefab;

    // Hàm này sẽ được gọi bởi Animation Event
    public void Shoot()
    {
        // Tạo ra viên đạn tại vị trí firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
