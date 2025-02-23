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
    public float budgetIncreaseRate = 0.5f;    // Lượng ngân sách tăng mỗi giây (tăng dần theo thời gian)
    public int maxEnemiesOnMap = 30;         // Số lượng enemy tối đa trên map
    public float maxOverdraft = 2f;          // Giới hạn nợ ngân sách tối đa (ngân sách có thể âm đến mức này)

    [Header("Discrete Budget Boost")]
    public float discreteBudgetBoost = 10f;  // Lượng ngân sách tăng đột biến
    public float boostInterval = 300f;       // Khoảng thời gian (giây) giữa các lần tăng đột biến (5 phút)

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
        StartCoroutine(BoostBudgetPeriodically());
    }

    void Update()
    {
        // Tăng ngân sách dần theo thời gian
        currentBudget += budgetIncreaseRate * Time.deltaTime;
    }

    // Coroutine để tăng ngân sách đột biến sau mỗi boostInterval (5 phút)
    private IEnumerator BoostBudgetPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(boostInterval);
            currentBudget += discreteBudgetBoost;
            Debug.Log($"Discrete budget boost applied! New budget: {currentBudget}");
        }
    }

    private IEnumerator DirectorLoop()
    {
        while (true)
        {
            // Kiểm tra số lượng enemy hiện có trên map
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemyCount < maxEnemiesOnMap)
            {
                // Tính toán danh sách enemy có thể spawn dựa trên ngân sách và chi phí của chúng
                List<EnemySpawnData> potentialEnemies = new List<EnemySpawnData>();
                List<float> effectiveWeights = new List<float>();
                float totalWeight = 0f;

                foreach (EnemySpawnData data in enemySpawnPool)
                {
                    // Nếu spawn enemy này sẽ khiến ngân sách vượt quá mức nợ cho phép, thì bỏ qua
                    if (currentBudget - data.cost < -maxOverdraft)
                        continue;

                    // Tính factor dựa trên tỉ lệ ngân sách hiện tại so với chi phí enemy
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

                    // Chọn ngẫu nhiên một spawn point có cooldown đã hết hạn
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

    // Hàm chọn ngẫu nhiên một spawn point có cooldown đã hết hạn
    private int GetAvailableSpawnPoint()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Time.time - spawnPointLastUsed[i] >= spawnPointCooldown)
            {
                availableIndices.Add(i);
            }
        }
        if (availableIndices.Count == 0)
            return -1;
        return availableIndices[Random.Range(0, availableIndices.Count)];
    }
}
