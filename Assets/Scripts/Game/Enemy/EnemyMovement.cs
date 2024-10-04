using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float moveDuration = 0.17f;
    [SerializeField] private float cooldownDuration = 0.3f;

    private NavMeshAgent agent;
    private float originalSpeed;
    private float moveTimer;
    private float cooldownTimer;
    private bool isMoving = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        originalSpeed = speed;
        agent.speed = speed;
        moveTimer = moveDuration;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            isMoving = false;
            agent.speed = 0f;
            cooldownTimer = cooldownDuration;
            return;
        }

        if (isMoving)
        {
            agent.SetDestination(target.position);
            moveTimer -= Time.deltaTime;

            if (moveTimer <= 0f)
            {
                StartCooldown();
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isMoving = true;
                agent.speed = speed;
                moveTimer = moveDuration;
            }
        }
    }

    private void StartCooldown()
    {
        isMoving = false;
        agent.speed = 0f;
        cooldownTimer = cooldownDuration;
    }
}
