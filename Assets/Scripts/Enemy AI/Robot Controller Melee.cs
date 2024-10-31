using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotControllerMelee : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;

    public float attackRange = 2f;     // Tầm cận chiến của quái vật
    public float attackDelay = 2f;     // Khoảng thời gian giữa các lần tấn công cận chiến
    private float attackTimer;
    private bool isAttacking = false;  // Biến trạng thái để biết có đang tấn công hay không

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

            if (distanceToPlayer > attackRange)
            {
                // Nếu đang tấn công nhưng người chơi ra khỏi tầm đánh, ngừng tấn công và di chuyển
                if (isAttacking)
                {
                    isAttacking = false;
                    animator.ResetTrigger("Attack");
                    animator.SetFloat("Velocity", 0.1f);  // Kích hoạt animation di chuyển
                }
                
                // Di chuyển về phía người chơi
                agent.SetDestination(Targetplayer.transform.position);
            }
            else
            {
                // Khi trong tầm cận chiến
                agent.SetDestination(transform.position);
                animator.SetFloat("Velocity", 0f);  // Ngừng animation di chuyển
                MeleeAttack();
            }
        }
    }

    void MeleeAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");  // Kích hoạt animation đòn tấn công cận chiến
            // Logic xử lý gây sát thương nếu người chơi nằm trong tầm
            // Ví dụ: Targetplayer.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            attackTimer = attackDelay;
        }
    }
}
