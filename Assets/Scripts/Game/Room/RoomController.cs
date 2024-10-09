using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    [Header("Start Room")]
    [SerializeField] private bool CanDestroyLastRoom = true;
    [SerializeField] private bool canPuzzle = true;

    [Header("Positions")]
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform bossPosition;

    [Header("CACHE")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Collider2D BarrierCollier;
    [SerializeField] private SpriteMask spriteMask;
    [SerializeField] private GameObject spikeTilemap;

    private List<GameObject> enemiesList = new List<GameObject>();
    private RoomManager roomManager;
    private BossMovement bossMovement;
    private bool hasTriggered;

    [Header("Appear")]
    [SerializeField] private DOTweenAnimation moveDOT;
    [SerializeField] private List<TilemapRenderer> tilemaps = new List<TilemapRenderer>();
    [SerializeField] private float dissolveSpeed = 1.0f;

    [Header("Door")]
    [SerializeField] private DoorController doorController;
    [SerializeField] private KeyDoor keyController;
    [SerializeField] private LayerMask collidersLayer;
    [SerializeField] private PolygonCollider2D roomCollider;

    private void Awake()
    {
        spikeTilemap.SetActive(Random.value > 0.5f);
        spriteMask.enabled = false;
        cinemachineCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        roomManager = FindFirstObjectByType<RoomManager>();
        bossMovement = FindFirstObjectByType<BossMovement>();
    }

    private void Start()
    {
        if (canPuzzle)
            StartPuzzle();
    }

    private void StartPuzzle()
    {
        bool havePuzzle = Random.value > 0.5f;

        if (havePuzzle)
        {
            doorController.SetDoor(false);

            Vector3 randomPosition = GetRandomClearPositionInRoom(collidersLayer);
            KeyDoor currentKey = Instantiate(keyController, randomPosition, Quaternion.identity, transform);
            currentKey.SetDoorController(doorController);
        }
    }
    #region Custom Methods
    private Vector3 GetNearPosition(Vector3 referencePosition)
    {
        float randomRadius, randomAngle;
        Vector3 nearPosition;

        for (int attempt = 0; attempt < 1000; attempt++)
        {
            randomRadius = Random.Range(2f, 4f);
            randomAngle = Random.Range(0f, 2f * Mathf.PI);

            float offsetX = Mathf.Cos(randomAngle) * randomRadius;
            float offsetY = Mathf.Sin(randomAngle) * randomRadius;

            nearPosition = new Vector3(referencePosition.x + offsetX, referencePosition.y + offsetY, referencePosition.z);

            if (roomCollider.OverlapPoint(nearPosition) && !CheckCollision(nearPosition, collidersLayer))
            {
                return SnapToGrid(nearPosition);
            }
        }
        return SnapToGrid(referencePosition);
    }

    private Vector3 GetRandomClearPositionInRoom(LayerMask layer)
    {
        Bounds bounds = roomCollider.bounds;

        for (int attempt = 0; attempt < 1000; attempt++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 randomPosition = SnapToGrid(new Vector3(randomX, randomY, 0));

            if (roomCollider.OverlapPoint(randomPosition) && !CheckCollision(randomPosition, layer))
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

    private bool CheckCollision(Vector3 position, LayerMask layer)
    {
        Collider2D hit = Physics2D.OverlapBox(position, new Vector2(0.8f, 0.8f), 0f, layer);
        return hit != null;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            BarrierCollier.enabled = true;

            roomManager.RemoveEnemiesLastRoom();
            DestroyLastRoom();
            moveDOT.DORestart();
            StartCoroutine(AnimateDissolveStep());

            SpawnRoom();
            MoveBoss();
        }
    }

    private IEnumerator AnimateDissolveStep()
    {
        float dissolveStep = 0f;
        float dissolveScale = 200f;

        while (dissolveStep < 1f || dissolveScale < 300f)
        {
            if (dissolveStep < 1f)
            {
                dissolveStep += Time.deltaTime * dissolveSpeed;
                dissolveStep = Mathf.Clamp01(dissolveStep);
            }

            if (dissolveScale < 300f)
            {
                dissolveScale += Time.deltaTime * dissolveSpeed * 100f;
                dissolveScale = Mathf.Clamp(dissolveScale, 200f, 300f);
            }

            foreach (var tile in tilemaps)
            {
                tile.material.SetFloat("_DissolveStep", dissolveStep);
                tile.material.SetFloat("_DissolveScale", dissolveScale);
            }

            yield return null;
        }
    }

    private void SpawnRoom()
    {
        roomManager.SpawnNewRoom(endPoint.position);
        roomManager.SetGridTarget(centerPosition);
    }

    private void DestroyLastRoom()
    {
        if (CanDestroyLastRoom)
        {
            roomManager.RemoveFirstRoomFromList();
        }
    }

    private void MoveBoss()
    {
        bossMovement.MoveBoss(bossPosition.position);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemiesList.Add(enemy);
    }

    public void RemoveEnemies()
    {
        foreach (var enemy in enemiesList)
        {
            Destroy(enemy);
        }
        enemiesList.Clear();
    }
}
