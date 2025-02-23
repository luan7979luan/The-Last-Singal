using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;           // Mảng chứa 3 prefab enemy
    public Transform[] spawnPoints;             // Các điểm spawn cho enemy

    [Header("Cài đặt Spawn")]
    public float initialSpawnInterval = 2f;      // Thời gian spawn ban đầu
    public float minSpawnInterval = 0.5f;         // Thời gian spawn tối thiểu
    public float spawnIntervalDecreaseRate = 0.01f; // Tốc độ giảm thời gian spawn theo thời gian
    public float randomSpawnVariance = 0.2f;      // Độ biến động ngẫu nhiên của thời gian spawn
    public float spawnPointCooldown = 1f;         // Thời gian chờ giữa các lần spawn tại cùng 1 điểm

    private float currentSpawnInterval;
    private float gameTimer = 0f;
    private float[] spawnPointLastSpawnTime;    // Lưu thời gian spawn cuối cùng của từng spawn point

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        spawnPointLastSpawnTime = new float[spawnPoints.Length];
        // Khởi tạo thời gian spawn của từng spawn point để có thể sử dụng ngay từ đầu
        for (int i = 0; i < spawnPointLastSpawnTime.Length; i++)
        {
            spawnPointLastSpawnTime[i] = -spawnPointCooldown;
        }
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        // Cập nhật thời gian game đã chạy
        gameTimer += Time.deltaTime;
        // Giảm dần thời gian spawn, nhưng không cho nhỏ hơn minSpawnInterval
        currentSpawnInterval = Mathf.Max(initialSpawnInterval - (gameTimer * spawnIntervalDecreaseRate), minSpawnInterval);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            // Thêm độ ngẫu nhiên cho thời gian spawn để tránh quá máy móc
            float spawnDelay = currentSpawnInterval * Random.Range(1f - randomSpawnVariance, 1f + randomSpawnVariance);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs != null && enemyPrefabs.Length > 0 && spawnPoints != null && spawnPoints.Length > 0)
        {
            // Chọn enemy prefab ngẫu nhiên từ mảng
            int prefabIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject selectedEnemyPrefab = enemyPrefabs[prefabIndex];

            // Lấy spawn point có cooldown đã hết hạn
            int spawnIndex = GetAvailableSpawnPoint();
            if (spawnIndex == -1)
            {
                // Nếu không có spawn point nào sẵn sàng, chọn ngẫu nhiên (tránh bị treo vòng lặp)
                spawnIndex = Random.Range(0, spawnPoints.Length);
            }

            // Cập nhật lại thời gian sử dụng của spawn point đã chọn
            spawnPointLastSpawnTime[spawnIndex] = Time.time;

            Transform spawnPoint = spawnPoints[spawnIndex];
            Instantiate(selectedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Spawned enemy {selectedEnemyPrefab.name} at: {spawnPoint.position} (Spawn Point {spawnIndex})");
        }
        else
        {
            Debug.LogWarning("EnemyPrefabs hoặc spawnPoints chưa được gán!");
        }
    }

    // Hàm kiểm tra và trả về spawn point có cooldown đã hết hạn
    private int GetAvailableSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Time.time - spawnPointLastSpawnTime[i] >= spawnPointCooldown)
            {
                return i;
            }
        }
        return -1; // Trả về -1 nếu không có spawn point nào đủ điều kiện
    }
}
