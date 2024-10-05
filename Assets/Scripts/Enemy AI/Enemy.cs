using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển của kẻ thù
    private Transform target; // Biến lưu trữ vị trí của người chơi

    void Update()
    {
        if (target != null)
        {
            MoveTowardsPlayer();
        }
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void MoveTowardsPlayer()
    {
        // Tính toán khoảng cách và hướng di chuyển
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
