using UnityEngine;

[System.Serializable]
public enum BossAttacks
{
    ExplodeArea, Enemy, ExplodePath, AreaAndPath, EnemyAndPath
}

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private MovementController playerMovement;

    [Header("Attacks")]
    [SerializeField] private BossAttacks attacks;
    [SerializeField] private Vector2 minMaxAttacksCooldown;
    private float attacksTimer;

    [Header("Explode Area Settings")]
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private float explosionCooldown = 5f;
    [SerializeField] private int gridRows = 3;
    [SerializeField] private int gridColumns = 3;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask explosionCollisionLayer;
    public static event System.EventHandler OnExplodeSpawn;
    public bool isPath { get; private set; }

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float enemyCooldown = 5f;
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private LayerMask enemyCollisionLayer;
    public static event System.EventHandler OnEnemySpawn;

    private BossMovement bossMovement;
    private float explosionCooldownTimer;
    private float enemyCooldownTimer;
    private Vector3 lastCheckedPosition;

    private void Start()
    {
        bossMovement = GetComponent<BossMovement>();
        explosionCooldownTimer = explosionCooldown;
        enemyCooldownTimer = enemyCooldown;
        attacksTimer = Random.Range(minMaxAttacksCooldown.x, minMaxAttacksCooldown.y);
    }

    private void Update()
    {
        //Path
        IsPathing();

        if (bossMovement.IsMoving) return;

        //Attacks
        attacksTimer -= Time.deltaTime;
        if(attacksTimer <= 0f)
        {
            SwitchAttack();
            attacksTimer = Random.Range(minMaxAttacksCooldown.x, minMaxAttacksCooldown.y);
        }

        //Explode Area
        if (attacks == BossAttacks.ExplodeArea || attacks == BossAttacks.AreaAndPath)
        {
            explosionCooldownTimer -= Time.deltaTime;

            if (explosionCooldownTimer <= 0f)
            {
                ExplodeArea();
                explosionCooldownTimer = explosionCooldown;
            }
        }

        //Enemy
        if (attacks == BossAttacks.Enemy || attacks == BossAttacks.EnemyAndPath)
        {
            enemyCooldownTimer -= Time.deltaTime;

            if (enemyCooldownTimer <= 0f)
            {
                SpawnEnemies();
                enemyCooldownTimer = enemyCooldown;
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

    private void SwitchAttack()
    {
        attacks = (BossAttacks)Random.Range(0, System.Enum.GetValues(typeof(BossAttacks)).Length);
    }

    #region Path
    private void SpawnExplosionInPlayer(object sender, System.EventArgs e)
    {
        if (bossMovement.IsMoving) return;

        if (attacks == BossAttacks.ExplodePath || attacks == BossAttacks.AreaAndPath || attacks == BossAttacks.EnemyAndPath)
        {
            Instantiate(explodePrefab, player.position, Quaternion.identity);
        }
    }

    private void IsPathing()
    {
        if ((attacks == BossAttacks.ExplodePath || attacks == BossAttacks.EnemyAndPath || attacks == BossAttacks.AreaAndPath) && !bossMovement.IsMoving)
            isPath = true;
        else
            isPath = false;
    }
    #endregion

    #region Explode Area
    private void ExplodeArea()
    {
        OnExplodeSpawn?.Invoke(this, System.EventArgs.Empty);
        Vector3 startPosition = GetRandomClearPositionAroundPlayer(explosionCollisionLayer, 8);

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

                if (!CheckCollision(spawnPosition, explosionCollisionLayer) && IsInCameraView(spawnPosition))
                {
                    Instantiate(explodePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    #endregion

    #region Spawn Enemies
    private void SpawnEnemies()
    {
        OnEnemySpawn?.Invoke(this, System.EventArgs.Empty);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomClearPositionAroundPlayer(enemyCollisionLayer, 8);

            if (spawnPosition == Vector3.zero)
            {
                Debug.LogWarning("Nenhum local sem colisão foi encontrado na visão da câmera para spawnar inimigos.");
                return;
            }

            if (IsInCameraView(spawnPosition)) // Verifica se está dentro da visão da câmera
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                roomManager.AddEnemy(enemy);
            }
            else
            {
                Debug.LogWarning("Posição fora da visão da câmera para spawnar inimigos.");
            }
        }
    }
    #endregion

    #region Custom Methods
    private Vector3 GetRandomClearPositionAroundPlayer(LayerMask layer, float radius)
    {
        Vector3 playerPosition = player.position;
        Vector2 inputDirection = playerMovement.GetInputDirection();

        if (inputDirection == Vector2.zero)
        {
            inputDirection = Random.insideUnitCircle.normalized;
        }

        for (int attempt = 0; attempt < 1000; attempt++)
        {
            Vector2 normalizedInputDirection = inputDirection.normalized;

            Vector3 randomPosition = playerPosition + new Vector3(
                normalizedInputDirection.x,
                normalizedInputDirection.y,
                0) * Random.Range(0f, radius);

            randomPosition = SnapToGrid(randomPosition);

            lastCheckedPosition = randomPosition;

            if (!CheckCollision(randomPosition, layer) && IsInCameraView(randomPosition))
            {
                return randomPosition;
            }
        }

        return Vector3.zero;
    }

    private bool IsInCameraView(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Floor(position.x) + 0.5f;
        float snappedY = Mathf.Floor(position.y) + 0.5f;
        return new Vector3(snappedX, snappedY, position.z);
    }

    private bool CheckCollision(Vector3 position, LayerMask layer)
    {
        Collider2D hit = Physics2D.OverlapBox(position, new Vector2(0.8f, 0.8f), 0f, layer);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(lastCheckedPosition, new Vector3(0.8f, 0.8f, 1));
    }
    #endregion
}
