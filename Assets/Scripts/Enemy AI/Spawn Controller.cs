using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UIElements;


public class SpawnController : MonoBehaviour{


  public GameObject enemyPrefab; // Kéo prefab kẻ thù vào đây trong Inspector
    public Transform player; // Kéo transform của người chơi vào đây
    public float spawnRadius = 10f; // Bán kính để spawn kẻ thù
    public float spawnDistance = 15f; // Khoảng cách xa hơn từ người chơi để spawn
    public int enemyCount = 5; // Số lượng kẻ thù cần spawn

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float randomAngle = UnityEngine.Random.Range(0f, 360f);
            float randomDistance = spawnDistance + UnityEngine.Random.Range(0f, spawnRadius);
            
            Vector3 spawnPosition = new Vector3(
                player.position.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                player.position.y, // Giữ y của người chơi
                player.position.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad) // Chuyển sang trục Z trong không gian 3D
            );

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetTarget(player); // Gọi hàm SetTarget trong script của kẻ thù
        }
    }
}