using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

public class PlayerCallbacks : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private DOTweenAnimation walkDOT;

    [Header("Death")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private MMF_Player deathFeedbacks;

    private void OnEnable()
    {
        MovementController.OnStartMove += StartMoveCallback;
        healthController.OnDeath += DeathCallback;
    }

    private void OnDisable()
    {
        MovementController.OnStartMove -= StartMoveCallback;
        healthController.OnDeath -= DeathCallback;
    }

    private void DeathCallback(object sender, System.EventArgs e)
    {
        deathFeedbacks.PlayFeedbacks();
    }

    private void StartMoveCallback(object sender, System.EventArgs e)
    {
        BurstWalkParticle();
        WalkDOTAnim();
    }

    private void BurstWalkParticle()
    {
        StartCoroutine(WalkParticle_COROUTINE());
    }

    private IEnumerator WalkParticle_COROUTINE()
    {
        var emission = walkParticle.emission;
        var burst = new ParticleSystem.Burst(0,7);
        walkParticle.Emit((int)burst.count.constant);
        yield return new WaitForSeconds(0.1f);
        burst.count = 0;
    }

    private void WalkDOTAnim()
    {
        walkDOT.DORestart();
    }
}
