using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = (Random.Range(1f, 3f));
    }

    void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {
        if (Targetplayer != null&& Vector3.Distance(transform.position, Targetplayer.transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(Targetplayer.transform.position);
            animator.SetFloat("Velocity", 0.5f);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetFloat("Velocity", 0f);  

            // trigger attack
            if(Targetplayer != null )
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}
