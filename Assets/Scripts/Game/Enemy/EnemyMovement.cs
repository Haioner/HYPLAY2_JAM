using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Grid Movement Settings")]
    [SerializeField] private float moveCooldown = 0.35f;
    [SerializeField] private float gridSize = 1.0f;

    [Header("Tilt")]
    [SerializeField] private float tiltAngle = 6f;
    [SerializeField] private float smoothTilt = 0.3f;

    [Header("Flip")]
    [SerializeField] private Transform flipHolder;

    private Seeker seeker;
    private AIPath aiPath;
    private Path path;
    private Transform target;
    private Vector3 targetPosition;
    private float moveTimer;
    private bool isMoving = false;
    private int currentWaypoint = 0;

    public static event System.EventHandler OnStartMove;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();

        aiPath.canMove = false;
        targetPosition = SnapToGrid(transform.position);
        moveTimer = moveCooldown;
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private void Update()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0 && !isMoving && path != null)
        {
            if (!isMoving)
                OnStartMove?.Invoke(this, System.EventArgs.Empty);

            MoveEnemy();
            moveTimer = moveCooldown;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;

            if (!isMoving)
            {
                MoveEnemy();
            }
        }
    }

    private void MoveEnemy()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            return;
        }

        Vector3 nextPosition = path.vectorPath[currentWaypoint];
        targetPosition = SnapToGrid(nextPosition);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Flip(direction);

        StartCoroutine(MoveToGridPosition(targetPosition));
        currentWaypoint++;
    }

    private void Flip(Vector3 direction)
    {
        float targetTilt = direction.x * tiltAngle;

        float currentTilt = flipHolder.localRotation.eulerAngles.z;
        if (currentTilt > 180f) currentTilt -= 360f;

        float newTilt = -Mathf.Lerp(currentTilt, targetTilt, smoothTilt);
        flipHolder.localRotation = Quaternion.Euler(0, 0, newTilt);

        if (direction.x < 0)
            flipHolder.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0)
            flipHolder.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator MoveToGridPosition(Vector3 gridTarget)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, gridTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, gridTarget, aiPath.maxSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = gridTarget;
        isMoving = false;
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Floor(position.x / gridSize) * gridSize + gridSize / 2;
        float y = Mathf.Floor(position.y / gridSize) * gridSize + gridSize / 2;
        float z = position.z;

        return new Vector3(x, y, z);
    }
}
