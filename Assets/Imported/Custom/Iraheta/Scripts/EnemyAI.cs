using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrolling, Idle, Chasing, Attacking }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDelay = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private bool isWaiting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = EnemyState.Patrolling;
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitBeforeNextPatrol());
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    IEnumerator WaitBeforeNextPatrol()
    {
        currentState = EnemyState.Idle;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2f);
        MoveToNextPatrolPoint();
    }

    void Idle()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(player.position);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
        else if (distanceToPlayer > detectionRange + 2f)
        {
            currentState = EnemyState.Patrolling;
            MoveToNextPatrolPoint();
        }
    }

    IEnumerator AttackRoutine()
    {
        currentState = EnemyState.Attacking;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackDelay);

        animator.SetBool("isAttacking", false);
        agent.isStopped = false;
        currentState = EnemyState.Chasing;
    }

    void AttackPlayer()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        currentState = EnemyState.Patrolling;
    }
}
