using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public float stopDistance = 0;
    private float repathRate = 0.5f;
    private float nextRepathTime = 0f;
    private Vector3 playerBypassPos, offSet;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        offSet = new Vector3(Random.Range(0.4f, 2f), 0, Random.Range(0.4f, 2f));
        
        agent.speed = 0.2f;
        agent.stoppingDistance = stopDistance;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;


        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found");
            return;
        }

        playerBypassPos = player.position;
        playerBypassPos.y = -9.424f;
        playerBypassPos += offSet;

        float distance = Vector3.Distance(transform.position, playerBypassPos);

        if (distance > stopDistance)
        {
            if (Time.time >= nextRepathTime)
            {
                agent.SetDestination(playerBypassPos);
                nextRepathTime = Time.time + repathRate;
            }
        }
        else
        {
            if (!agent.isStopped)
            {
                agent.ResetPath();
            }
        }
    }

}
