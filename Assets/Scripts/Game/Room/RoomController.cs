using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("Appear")]
    [SerializeField] private DOTweenAnimation moveDOT;
    [SerializeField] private List<TilemapRenderer> tilemaps = new List<TilemapRenderer>();
    [SerializeField] private float dissolveSpeed = 1.0f;

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
            moveDOT.DORestart();
            StartCoroutine(AnimateDissolveStep());

            hasTriggered = true;
            BarrierCollier.enabled = true;
            SpawnRoom();
            DestroyLastRoom();
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
            roomManager.RemoveFirstRoomFromList();
    }

    private void MoveBoss()
    {
        bossMovement.MoveBoss(bossPosition.position);
    }
}
