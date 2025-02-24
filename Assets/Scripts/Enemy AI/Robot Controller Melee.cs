using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotControllerMelee : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;

    public float attackRange = 2f;    // Tầm tấn công cận chiến của quái vật
    public float attackDelay = 0.5f;    // Khoảng thời gian giữa các lần tấn công
    private float attackTimer;

    // Tham chiếu đến đối tượng MeleeZone chứa script NPC_MeleeDamageZone
    public NPC_MeleeDamageZone meleeDamageZone;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = Random.Range(3f, 6f);
        attackTimer = attackDelay;

        if (Targetplayer == null)
        {
            Targetplayer = GameObject.FindGameObjectWithTag("Player");
        }
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
                // Di chuyển về phía người chơi nếu ngoài tầm tấn công
                agent.SetDestination(Targetplayer.transform.position);
                animator.SetFloat("Velocity", 0.5f);
            }
            else
            {
                // Dừng lại và tấn công khi trong tầm tấn công
                agent.SetDestination(transform.position);
                animator.SetFloat("Velocity", 0f);

                RotateTowardsPlayer();
                MeleeAttack();
            }
        }
    }

    // Phương thức tấn công melee
    void MeleeAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            // Kích hoạt animation tấn công
            animator.SetTrigger("Attack");

            // Reset lại vùng damage qua Animation Event hoặc trực tiếp (nếu không dùng Animation Event)
            if (meleeDamageZone != null)
            {
                meleeDamageZone.EnableDamageZone();
            }
        }
    }

    // Quay hướng enemy về phía người chơi
    void RotateTowardsPlayer()
    {
        Vector3 direction = (Targetplayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    

}
