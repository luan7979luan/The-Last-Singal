using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // Prefab của enemy
    public Transform[] spawnPoints;      // Danh sách các điểm spawn
    public float spawnInterval = 60f;    // Thời gian spawn mỗi 1 phút (60 giây)

    private int spawnCount = 5;          // Số lượng enemy spawn ban đầu
    private int maxSpawnCount = 20;      // Giới hạn số lượng enemy spawn tối đa
    private float elapsedTime = 0f;      // Biến theo dõi thời gian đã trôi qua

    void Start()
    {
        StartCoroutine(SpawnEnemies()); // Bắt đầu coroutine để spawn
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) // Lặp vô hạn
        {
            yield return new WaitForSeconds(spawnInterval); // Đợi 1 phút

            // Spawn nhiều enemy trong mỗi vòng lặp
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
            }

            // Tăng số lượng enemy spawn sau mỗi lần spawn
            spawnCount = Mathf.Min(spawnCount + 1, maxSpawnCount); // Đảm bảo không vượt quá số lượng tối đa

            // Cập nhật thời gian đã trôi qua để kiểm tra số lượng enemy spawn tăng dần
            elapsedTime += spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Chọn ngẫu nhiên một điểm spawn
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn enemy tại điểm spawn đã chọn
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
