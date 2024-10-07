using UnityEngine;

[System.Serializable]
public enum BossAttacks
{
    ExplodeArea, Enemy, ExplodePath
}

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Header("Attacks")]
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private BossAttacks attacks;

    [Header("Explode Area Settings")]
    [SerializeField] private float explosionCooldown = 5f;
    [SerializeField] private int gridRows = 3;
    [SerializeField] private int gridColumns = 3;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask collisionLayer;

    private float cooldownTimer;
    private Vector3 lastCheckedPosition;

    private void Start()
    {
        cooldownTimer = explosionCooldown;
    }

    private void Update()
    {
        if (attacks == BossAttacks.ExplodeArea)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                ExplodeArea();
                cooldownTimer = explosionCooldown;
            }
        }
    }

    private void OnEnable()
    {
        MovementController.OnEndMove += SpawnExplosionInPlayer;
    }

    private void OnDisable()
    {
        MovementController.OnEndMove -= SpawnExplosionInPlayer;
    }

    #region Path
    private void SpawnExplosionInPlayer(object sender, System.EventArgs e)
    {
        if (attacks == BossAttacks.ExplodePath)
        {
            Instantiate(explodePrefab, player.position, Quaternion.identity);
        }
    }
    #endregion

    #region Explode Area
    private void ExplodeArea()
    {
        Vector3 startPosition = GetRandomClearPositionInView();
        if (startPosition == Vector3.zero)
        {
            Debug.LogWarning("Nenhum local sem colisão foi encontrado na visão da câmera.");
            return;
        }

        int randRow = Random.Range(1, gridRows);
        int randColumns = Random.Range(1, gridColumns);
        for (int row = 0; row < randRow; row++)
        {
            for (int col = 0; col < randColumns; col++)
            {
                Vector3 spawnPosition = new Vector3(
                    startPosition.x + (col * gridSize),
                    startPosition.y + (row * gridSize),
                    startPosition.z
                );

                spawnPosition = SnapToGrid(spawnPosition);

                if (!CheckCollision(spawnPosition))
                {
                    Instantiate(explodePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    private Vector3 GetRandomClearPositionInView()
    {
        Camera cam = Camera.main;
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        for (int attempt = 0; attempt < 1000; attempt++)
        {
            float randomX = Random.Range(min.x, max.x);
            float randomY = Random.Range(min.y, max.y);
            Vector3 randomPosition = SnapToGrid(new Vector3(randomX, randomY, 0));

            lastCheckedPosition = randomPosition;

            if (!CheckCollision(randomPosition))
            {
                return randomPosition;
            }
        }

        return Vector3.zero;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Floor(position.x) + 0.5f;
        float snappedY = Mathf.Floor(position.y) + 0.5f;
        return new Vector3(snappedX, snappedY, position.z);
    }

    private bool CheckCollision(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapBox(position, new Vector2(0.8f, 0.8f), 0f, collisionLayer);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(lastCheckedPosition, new Vector3(0.8f, 0.8f, 1));
    }
    #endregion
}
