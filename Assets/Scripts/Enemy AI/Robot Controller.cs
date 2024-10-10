using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public Transform player;        // Reference to the player
    private NavMeshAgent agent;     // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component on this enemy
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Set the agent's destination to the player's position every frame
        agent.SetDestination(player.position);
    }
}
