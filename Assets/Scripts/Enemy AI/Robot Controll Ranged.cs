using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;

    public GameObject bulletPrefab;   // Thêm prefab viên đạn
    public Transform firePoint;       // Điểm bắn đạn ra

    public float timer = 5;
    private float bullettime;

    public float shootingRange = 10f; // Tầm bắn của quái vật
    public float attackDelay = 2f;    // Khoảng thời gian giữa các lần bắn
    private float attackTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = Random.Range(1f, 3f);
        attackTimer = attackDelay;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
{
    if (Targetplayer != null)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Targetplayer.transform.position);

        // Nếu người chơi ngoài tầm dừng và trong tầm bắn
        if (distanceToPlayer > agent.stoppingDistance)
        {
            // Di chuyển về phía người chơi
            agent.SetDestination(Targetplayer.transform.position);
            animator.SetFloat("Velocity", 0.5f);  // Kích hoạt animation di chuyển
        }
        else
        {
            // Dừng lại và bắn khi người chơi ở gần
            agent.SetDestination(transform.position);
            animator.SetFloat("Velocity", 0f);  // Ngừng animation di chuyển

            // Quay về phía người chơi và bắn
            RotateTowardsPlayer();
            shootatplayer();
        }
    }
}

    void shootatplayer()
{
    bullettime += Time.deltaTime;

    if (bullettime >= timer)
    {
        // Kích hoạt animation tấn công
        animator.SetTrigger("Attack");
        bullettime = 0;

        // Xác định hướng bắn về phía người chơi
        Vector3 directionToPlayer = (Targetplayer.transform.position - firePoint.position).normalized;

        // Xoay firePoint theo hướng của người chơi
        firePoint.rotation = Quaternion.LookRotation(directionToPlayer);

        // Tạo ra viên đạn tại vị trí firePoint và hướng về phía người chơi
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Nếu bạn muốn thiết lập tốc độ hoặc di chuyển viên đạn
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = directionToPlayer * 20f; // 20f là tốc độ, bạn có thể thay đổi
        }
    }
}

    // Quay mặt quái vật về phía người chơi
    void RotateTowardsPlayer()
    {
        Vector3 direction = (Targetplayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
