using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject[] monsterPrefabs;
    public Transform spawnPoint;

    public float castCooldown = 5f;
    private float lastCastTime;
    
    public int summonCount = 3;
    public float spawnRadius = 2f;

    // Biến lưu đối tượng người chơi (không cần kéo thả nếu tìm tự động)
    public Transform player;
    public float rotationSpeed = 5f;

    void Awake()
    {
        // Tắt xoay tự động của NavMesh Agent để tự điều khiển xoay
        agent.updateRotation = false;
    }

    void Start()
    {
        lastCastTime = -castCooldown;
        // Nếu player chưa được gán qua Inspector, tự động tìm kiếm theo tag "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            // Tính hướng từ boss đến người chơi
            Vector3 direction = (player.position - transform.position).normalized;
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        if (agent.velocity.magnitude > 0.1f)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("cast");
            lastCastTime = Time.time;
        }
    }

    public void SummonMonster()
    {
        for (int i = 0; i < summonCount; i++)
        {
            int index = Random.Range(0, monsterPrefabs.Length);
            Vector3 randomOffset = new Vector3(Random.insideUnitSphere.x, 0, Random.insideUnitSphere.z).normalized * Random.Range(0, spawnRadius);
            Vector3 spawnPos = spawnPoint.position + randomOffset;
            Instantiate(monsterPrefabs[index], spawnPos, spawnPoint.rotation);
        }
    }
}
