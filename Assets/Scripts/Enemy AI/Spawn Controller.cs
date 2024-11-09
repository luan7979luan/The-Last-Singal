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

    private int currentWave = 0;             // Biến theo dõi số wave hiện tại
    private int currentEnemyCount = 0;       // Biến theo dõi số lượng enemy đã spawn trong wave
    private float waveTimer = 0f;            // Thời gian để spawn wave mới

    void Start()
    {
        // Bắt đầu spawn ngay khi game bắt đầu
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        // Loop qua các wave
        while (currentWave < 10) // Giới hạn số wave (hoặc có thể thay đổi)
        {
            currentWave++; // Tăng số wave
            currentEnemyCount = Mathf.Min(initialEnemyCount * currentWave, maxEnemyCount);

            // Debug log số lượng enemy trong wave
            Debug.Log("Wave " + currentWave + " - Spawning " + currentEnemyCount + " enemies");

            // Spawn các enemy trong wave
            for (int i = 0; i < currentEnemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval); // Đợi một chút trước khi spawn tiếp con quái mới
            }

            // Đợi trước khi bắt đầu wave tiếp theo
            yield return new WaitForSeconds(difficultyIncreaseTime); // Đợi thời gian giữa các wave
        }
    }

    private void SpawnEnemy()
    {
        // Kiểm tra để đảm bảo spawn đúng
        if (enemyPrefab != null && spawnPoints.Length > 0)
        {
            // Chọn ngẫu nhiên điểm spawn
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn enemy tại điểm spawn đã chọn
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Debug log để kiểm tra spawn
            Debug.Log("Spawned enemy at: " + spawnPoint.position);
        }
        else
        {
            Debug.LogWarning("EnemyPrefab hoặc spawnPoints chưa được gán!");
        }
    }
}
