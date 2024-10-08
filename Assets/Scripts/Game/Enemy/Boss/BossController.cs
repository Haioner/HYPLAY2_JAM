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
    [SerializeField] private BossAttacks attacks;
    [SerializeField] private Vector2 minMaxAttacksCooldown;
    private float attacksTimer;

    [Header("Explode Area Settings")]
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private float explosionCooldown = 5f;
    [SerializeField] private int gridRows = 3;
    [SerializeField] private int gridColumns = 3;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask collisionLayer;
    public static event System.EventHandler OnExplodeSpawn;
    public bool isPath { get; private set; }

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float enemyCooldown = 5f;
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private RoomManager roomManager;
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
        if (bossMovement.IsMoving) return;

        //Attacks
        attacksTimer -= Time.deltaTime;
        if(attacksTimer <= 0f)
        {
            SwitchAttack();
            attacksTimer = Random.Range(minMaxAttacksCooldown.x, minMaxAttacksCooldown.y);
        }

        //Path
        IsPathing();

        //Explode Area
        if (attacks == BossAttacks.ExplodeArea)
        {
            explosionCooldownTimer -= Time.deltaTime;

            if (explosionCooldownTimer <= 0f)
            {
                ExplodeArea();
                explosionCooldownTimer = explosionCooldown;
            }
        }

        //Enemy
        if (attacks == BossAttacks.Enemy)
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
        if (attacks == BossAttacks.ExplodePath)
        {
            Instantiate(explodePrefab, player.position, Quaternion.identity);
        }
    }

    private void IsPathing()
    {
        if (attacks == BossAttacks.ExplodePath)
            isPath = true;
        else
            isPath = false;
    }
    #endregion

    #region Explode Area
    private void ExplodeArea()
    {
        OnExplodeSpawn?.Invoke(this, System.EventArgs.Empty);
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
    #endregion

    #region Spawn Enemies
    private void SpawnEnemies()
    {
        OnEnemySpawn?.Invoke(this, System.EventArgs.Empty);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomClearPositionInView();

            if (spawnPosition == Vector3.zero)
            {
                Debug.LogWarning("Nenhum local sem colisão foi encontrado na visão da câmera para spawnar inimigos.");
                return;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            roomManager.AddEnemy(enemy);
        }
    }
    #endregion

    #region Custom Methods
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
