using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Thông tin của mỗi loại enemy
[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public float cost = 1f;      // Chi phí spawn enemy này
    public float weight = 1f;    // Trọng số cơ bản dùng cho lựa chọn ngẫu nhiên
}

public class Director : MonoBehaviour
{
    public EnemySpawnData[] enemySpawnPool; // Danh sách enemy mà Director có thể spawn
    public Transform[] spawnPoints;         // Các điểm spawn

    [Header("Director Settings")]
    public float initialBudget = 5f;         // Ngân sách ban đầu
    public float budgetIncreaseRate = 0.5f;    // Lượng ngân sách tăng mỗi giây
    public int maxEnemiesOnMap = 30;         // Số lượng enemy tối đa trên map
    public float maxOverdraft = 2f;          // Giới hạn nợ ngân sách tối đa (ngân sách có thể âm đến mức này)

    [Header("Spawn Timing")]
    public float spawnCheckInterval = 0.5f;  // Khoảng thời gian giữa các lần kiểm tra spawn

    [Header("Spawn Point Cooldown")]
    public float spawnPointCooldown = 1f;    // Thời gian chờ trước khi một điểm spawn được sử dụng lại

    private float currentBudget;
    private float[] spawnPointLastUsed;      // Lưu thời gian spawn cuối cùng của từng spawn point

    void Start()
    {
        currentBudget = initialBudget;
        spawnPointLastUsed = new float[spawnPoints.Length];
        for (int i = 0; i < spawnPointLastUsed.Length; i++)
        {
            spawnPointLastUsed[i] = -spawnPointCooldown;
        }
        StartCoroutine(DirectorLoop());
    }

    void Update()
    {
        // Tăng ngân sách dần theo thời gian
        currentBudget += budgetIncreaseRate * Time.deltaTime;
    }

    private IEnumerator DirectorLoop()
    {
        while (true)
        {
            // Giới hạn số lượng enemy trên map
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemyCount < maxEnemiesOnMap)
            {
                // Thay vì lọc chỉ enemy có chi phí <= ngân sách, ta xem xét toàn bộ enemy và tính trọng số hiệu quả
                List<EnemySpawnData> potentialEnemies = new List<EnemySpawnData>();
                List<float> effectiveWeights = new List<float>();
                float totalWeight = 0f;

                foreach (EnemySpawnData data in enemySpawnPool)
                {
                    // Nếu spawn enemy này sẽ làm ngân sách vượt quá mức nợ cho phép, thì bỏ qua
                    if (currentBudget - data.cost < -maxOverdraft)
                        continue;

                    // Tính factor dựa trên tỉ lệ ngân sách hiện tại so với chi phí enemy
                    // Nếu ngân sách thấp, factor giảm xuống (nhưng tối thiểu 0.1 để vẫn có cơ hội spawn)
                    float factor = currentBudget / data.cost;
                    factor = Mathf.Clamp(factor, 0.1f, 1f);
                    float effectiveWeight = data.weight * factor;
                    
                    potentialEnemies.Add(data);
                    effectiveWeights.Add(effectiveWeight);
                    totalWeight += effectiveWeight;
                }

                if (potentialEnemies.Count > 0)
                {
                    // Lựa chọn enemy theo weighted random dựa trên effectiveWeights
                    float randomValue = Random.Range(0f, totalWeight);
                    EnemySpawnData selectedData = null;
                    for (int i = 0; i < potentialEnemies.Count; i++)
                    {
                        randomValue -= effectiveWeights[i];
                        if (randomValue <= 0f)
                        {
                            selectedData = potentialEnemies[i];
                            break;
                        }
                    }

                    // Kiểm tra spawn point khả dụng (không nằm trong cooldown)
                    int spawnIndex = GetAvailableSpawnPoint();
                    if (spawnIndex == -1)
                    {
                        yield return new WaitForSeconds(spawnCheckInterval);
                        continue;
                    }

                    // Spawn enemy và cập nhật ngân sách (có thể dẫn đến ngân sách âm, nhưng không quá maxOverdraft)
                    Transform spawnPoint = spawnPoints[spawnIndex];
                    Instantiate(selectedData.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    currentBudget -= selectedData.cost;
                    spawnPointLastUsed[spawnIndex] = Time.time;

                    Debug.Log($"Director spawned {selectedData.enemyPrefab.name} at {spawnPoint.position}. Budget now: {currentBudget}");
                }
            }
            yield return new WaitForSeconds(spawnCheckInterval);
        }
    }

    // Hàm trả về index của spawn point đã hết cooldown, nếu không có thì trả về -1
    private int GetAvailableSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Time.time - spawnPointLastUsed[i] >= spawnPointCooldown)
                return i;
        }
        return -1;
    }
}
