using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotControllerMelee : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;
    //public PlayerHealth health;

    public int damage;
    //public NPC_DamageZone _damageZone;

    public float attackRange = 2f;   // Tầm tấn công cận chiến của quái vật
    public float attackDelay = 2f;   // Khoảng thời gian giữa các lần tấn công
    private float attackTimer;

    private void Awake()
    {
        //Khởi tạo DamageZone
        //_damageZone = GetComponentInChildren<NPC_DamageZone>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //health = GetComponent<PlayerHealth>();
        agent.speed = Random.Range(1f, 3f);
        attackTimer = attackDelay;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void EnableDamageCaster()
    {
        //_damageZone.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        //_damageZone.DisableDamageCaster();
    }

    private void Move()
    {
        if (Targetplayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Targetplayer.transform.position);

            if (distanceToPlayer > attackRange)
            {
                // Di chuyển về phía người chơi nếu ngoài tầm tấn công
                agent.SetDestination(Targetplayer.transform.position);
                animator.SetFloat("Velocity", 0.5f);  // Kích hoạt animation di chuyển
            }
            else
            {
                // Dừng lại và tấn công khi trong tầm tấn công
                agent.SetDestination(transform.position);
                animator.SetFloat("Velocity", 0f);  // Ngừng animation di chuyển

                // Quay về phía người chơi và tấn công
                RotateTowardsPlayer();
                MeleeAttack();
            }
        }
    }

    // Tấn công cận chiến người chơi
    void MeleeAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            // Kích hoạt animation tấn công
            animator.SetTrigger("Attack");
            attackTimer = 0;

            // Kiểm tra nếu người chơi trong tầm tấn công để gây sát thương
            
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
