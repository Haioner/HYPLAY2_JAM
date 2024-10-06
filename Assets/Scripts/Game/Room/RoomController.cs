using Unity.Cinemachine;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private bool CanDestroyLastRoom = true;

    [Header("Positions")]
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform bossPosition;

    [Header("CACHE")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Collider2D BarrierCollier;

    private RoomManager roomManager;
    private BossMovement bossMovement;
    private bool hasTriggered;

    private void Awake()
    {
        cinemachineCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        roomManager = FindFirstObjectByType<RoomManager>();
        bossMovement = FindFirstObjectByType<BossMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            BarrierCollier.enabled = true;
            SpawnRoom();
            DestroyLastRoom();
            MoveBoss();
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
            roomManager.RemoveFirstRoomFromList();
    }

    private void MoveBoss()
    {
        bossMovement.MoveBoss(bossPosition.position);
    }
}
