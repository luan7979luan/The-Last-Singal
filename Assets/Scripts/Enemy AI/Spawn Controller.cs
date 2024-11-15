using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;           // Prefab của enemy
    public Transform[] spawnPoints;          // Các điểm spawn cho enemy
    public float spawnInterval = 1f;         // Thời gian giữa các lần spawn trong mỗi wave
    public int initialEnemyCount = 5;        // Số lượng enemy spawn trong wave đầu tiên
    public int maxEnemyCount = 50;           // Giới hạn số lượng enemy tối đa trong game
    public float difficultyIncreaseTime = 60f; // Thời gian giữa mỗi lần tăng độ khó
    public float difficultyMultiplier = 1.2f; // Hệ số tăng độ khó sau mỗi wave
    public int minimumEnemyOnMap = 5;        // Số lượng enemy tối thiểu trên map

    private int currentWave = 0;             // Biến theo dõi số wave hiện tại
    private int currentEnemyCount = 0;       // Biến theo dõi số lượng enemy đã spawn trong wave
    private float waveTimer = 0f;            // Thời gian để spawn wave mới
    private int lastSpawnIndex = -1; // Theo dõi điểm spawn trước đó để tránh lặp lại

    void Start()
    {
        // Bắt đầu spawn ngay khi game bắt đầu
        StartCoroutine(SpawnWave());
        StartCoroutine(CheckMinimumEnemies());
    }

    private IEnumerator SpawnWave()
    {
        while (true) // Lặp vô tận để liên tục spawn wave
        {
            currentWave++; // Tăng số wave
            currentEnemyCount = Mathf.Min(initialEnemyCount * currentWave, maxEnemyCount);

            Debug.Log("Wave " + currentWave + " - Spawning " + currentEnemyCount + " enemies");

            // Spawn các enemy trong wave
            for (int i = 0; i < currentEnemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval); // Đợi giữa các lần spawn
            }

            // Đợi trước khi bắt đầu wave tiếp theo
            yield return new WaitForSeconds(difficultyIncreaseTime);
        }
    }

    private IEnumerator CheckMinimumEnemies()
    {
        while (true) // Kiểm tra liên tục
        {
            yield return new WaitForSeconds(2f); // Kiểm tra sau mỗi 2 giây

            // Đếm số lượng enemy hiện có trên map
            int currentEnemyOnMap = GameObject.FindGameObjectsWithTag("Enemy").Length;

            // Spawn thêm nếu số lượng enemy dưới mức tối thiểu
            if (currentEnemyOnMap < minimumEnemyOnMap)
            {
                int enemiesToSpawn = minimumEnemyOnMap - currentEnemyOnMap;
                Debug.Log("Spawning additional " + enemiesToSpawn + " enemies to meet the minimum requirement.");

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemy();
                }
            }
        }
    }

    private void SpawnEnemy()
    {
       if (enemyPrefab != null && spawnPoints.Length > 0)
    {
        // Chọn một điểm spawn ngẫu nhiên, tránh trùng với lần trước
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        } while (randomIndex == lastSpawnIndex && spawnPoints.Length > 1);

        lastSpawnIndex = randomIndex; // Cập nhật điểm spawn cuối cùng

        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn enemy tại điểm spawn đã chọn
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"Spawned enemy at: {spawnPoint.position} (Spawn Point {randomIndex})");
    }
    else
    {
        Debug.LogWarning("EnemyPrefab hoặc spawnPoints chưa được gán!");
    }
    }
}
