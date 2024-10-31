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
            animator.SetFloat("Velocity", 0.1f);  // Kích hoạt animation di chuyển
        }
        else
        {
            // Dừng lại và bắn khi người chơi ở gần
            agent.SetDestination(transform.position);
            animator.SetFloat("Velocity", 0f);  // Ngừng animation di chuyển

  
}
    }
}

}
