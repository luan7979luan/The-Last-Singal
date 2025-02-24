using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Discrete Budget Boost")]
    public float discreteBudgetBoost = 10f;  // Lượng ngân sách tăng đột biến
    public float boostInterval = 300f;       // Khoảng thời gian (giây) giữa các lần tăng đột biến (5 phút)

    [Header("High-Cost Enemy Spawn")]
    public float expensiveEnemySpawnInterval = 30f; // Mỗi 30 giây spawn ít nhất 1 enemy cao tiền
    public float expensiveCostThreshold = 2f;       // Ngưỡng chi phí để xác định enemy cao tiền
    private float lastExpensiveSpawnTime = 0f;        // Thời gian spawn enemy cao tiền cuối cùng

    [Header("Spawn Timing")]
    public float spawnCheckInterval = 0.5f;  // Khoảng thời gian giữa các lần kiểm tra spawn

    [Header("Spawn Point Cooldown")]
    public float spawnPointCooldown = 1f;    // Thời gian chờ trước khi một điểm spawn được sử dụng lại

    [Header("Health Scaling Settings")]
    public float healthScalingInterval = 300f; // Mỗi 5 phút enemy tăng thêm sức mạnh
    public float healthScalingMultiplierPerInterval = 0.2f; // Mỗi interval, enemy tăng 20% máu

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

    // Tính toán hệ số scaling dựa trên thời gian đã trôi qua
    private float GetCurrentHealthMultiplier()
    {
        int intervalsPassed = Mathf.FloorToInt(Time.timeSinceLevelLoad / healthScalingInterval);
        return 1f + intervalsPassed * healthScalingMultiplierPerInterval;
    }

    // Áp dụng scaling cho lượng máu của enemy ngay sau khi spawn
    private void ApplyHealthScaling(GameObject enemy)
{
    Health enemyHealth = enemy.GetComponent<Health>();
    if (enemyHealth != null)
    {
        float multiplier = GetCurrentHealthMultiplier();
        enemyHealth.maxHealth *= multiplier;
        enemyHealth.currentHealth = enemyHealth.maxHealth;
        Debug.Log($"Applied health scaling: multiplier = {multiplier}, new maxHealth = {enemyHealth.maxHealth}");

        // Tính enemy level dựa trên số khoảng thời gian đã trôi qua (ví dụ mỗi 5 phút tăng 1 level)
        int enemyLevel = 1 + Mathf.FloorToInt(Time.timeSinceLevelLoad / healthScalingInterval);
        
        // Cập nhật UI hiển thị level nếu có component UIHealthBarRanged
        UIHealthBarRanged ui = enemy.GetComponentInChildren<UIHealthBarRanged>();
        if (ui != null)
        {
            ui.enemyLevel = enemyLevel;
            ui.levelText.text = enemyLevel.ToString("00");
        }
    }
}


    private IEnumerator DirectorLoop()
    {
        while (true)
        {
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemyCount < maxEnemiesOnMap)
            {
                bool forcedHighCostSpawn = false;
                if (Time.time - lastExpensiveSpawnTime > expensiveEnemySpawnInterval)
                {
                    List<EnemySpawnData> highCostEnemies = new List<EnemySpawnData>();
                    List<float> effectiveWeightsHigh = new List<float>();
                    float totalWeightHigh = 0f;
                    foreach (EnemySpawnData data in enemySpawnPool)
                    {
                        if (data.cost >= expensiveCostThreshold && currentBudget - data.cost >= -maxOverdraft)
                        {
                            float factor = currentBudget / data.cost;
                            factor = Mathf.Clamp(factor, 0.1f, 1f);
                            float effectiveWeight = data.weight * factor;
                            highCostEnemies.Add(data);
                            effectiveWeightsHigh.Add(effectiveWeight);
                            totalWeightHigh += effectiveWeight;
                        }
                    }
                    if (highCostEnemies.Count > 0)
                    {
                        float randomValue = Random.Range(0f, totalWeightHigh);
                        EnemySpawnData selectedData = null;
                        for (int i = 0; i < highCostEnemies.Count; i++)
                        {
                            randomValue -= effectiveWeightsHigh[i];
                            if (randomValue <= 0f)
                            {
                                selectedData = highCostEnemies[i];
                                break;
                            }
                        }
                        int spawnIndex = GetAvailableSpawnPoint();
                        if (spawnIndex != -1)
                        {
                            Transform spawnPoint = spawnPoints[spawnIndex];
                            GameObject enemy = Instantiate(selectedData.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                            currentBudget -= selectedData.cost;
                            spawnPointLastUsed[spawnIndex] = Time.time;
                            ApplyHealthScaling(enemy); // Điều chỉnh lượng máu theo thời gian
                            lastExpensiveSpawnTime = Time.time;
                            Debug.Log($"Director forced spawn high-cost enemy {selectedData.enemyPrefab.name} at {spawnPoint.position}. Budget now: {currentBudget}");
                            forcedHighCostSpawn = true;
                        }
                    }
                }

                if (!forcedHighCostSpawn)
                {
                    List<EnemySpawnData> potentialEnemies = new List<EnemySpawnData>();
                    List<float> effectiveWeights = new List<float>();
                    float totalWeight = 0f;
                    foreach (EnemySpawnData data in enemySpawnPool)
                    {
                        if (currentBudget - data.cost < -maxOverdraft)
                            continue;
                        float factor = currentBudget / data.cost;
                        factor = Mathf.Clamp(factor, 0.1f, 1f);
                        float effectiveWeight = data.weight * factor;
                        potentialEnemies.Add(data);
                        effectiveWeights.Add(effectiveWeight);
                        totalWeight += effectiveWeight;
                    }
                    if (potentialEnemies.Count > 0)
                    {
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
                        int spawnIndex = GetAvailableSpawnPoint();
                        if (spawnIndex != -1)
                        {
                            Transform spawnPoint = spawnPoints[spawnIndex];
                            GameObject enemy = Instantiate(selectedData.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                            currentBudget -= selectedData.cost;
                            spawnPointLastUsed[spawnIndex] = Time.time;
                            ApplyHealthScaling(enemy);
                            Debug.Log($"Director spawned {selectedData.enemyPrefab.name} at {spawnPoint.position}. Budget now: {currentBudget}");
                        }
                    }
                }
            }
            yield return new WaitForSeconds(spawnCheckInterval);
        }
    }

    // Hàm chọn ngẫu nhiên spawn point có cooldown đã hết hạn
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
