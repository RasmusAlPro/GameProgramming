using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 3;
    public float speedRun = 7;
    public float viewRadius = 5;
    public float viewAngle = 90;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime;
    private Transform player;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;
    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.ResetPath();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No object with Player tag found!");
        }
        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    void Update()
    {
        EnviromentView();

        // Feed speed to the Animator
        if (animator != null)
        {
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
        }

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Attack when close enough
        if (distanceToPlayer <= attackDistance)
        {
            Stop();
            m_CaughtPlayer = true;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (Time.time >= nextAttackTime)
            {
                if (animator != null)
                {
                    animator.SetTrigger("attack");
                }

                nextAttackTime = Time.time + attackCooldown;
            }

            return;
        }

        // Chase player
        m_CaughtPlayer = false;
        m_PlayerNear = false;

        if (m_PlayerInRange) 
        {
            // Guard sees you: Move toward your actual position
            Move(speedRun);
            navMeshAgent.SetDestination(player.position);
        }
        else 
        {
            // Guard lost you: Move toward the LAST place he saw you
            Move(speedRun);
            navMeshAgent.SetDestination(playerLastPosition);

            // If guard reached last known position and player is far away, return to patrol
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0 && distanceToPlayer >= 6f)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;

                    Move(speedWalk);

                    m_TimeToRotate = timeToRotate;
                    m_WaitTime = startWaitTime;

                    NextPoint();
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        if (waypoints.Length == 0) return;
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3f)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                NextPoint();
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        m_PlayerInRange = false;

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                    m_PlayerPosition = player.position;
                    playerLastPosition = m_PlayerPosition;
                }
            }
        }
    }
}