using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyCallbacks : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private DOTweenAnimation walkDOT;

    [Header("Attack")]
    [SerializeField] private MeeleAttack meeleAttack;

    private void OnEnable()
    {
        EnemyMovement.OnStartMove += StartMoveCallback;
        meeleAttack.OnTriggerAttack += StartAttackCallback;
    }

    private void OnDisable()
    {
        EnemyMovement.OnStartMove -= StartMoveCallback;
        meeleAttack.OnTriggerAttack -= StartAttackCallback;
    }

    #region Attack
    private void StartAttackCallback(object sender, System.EventArgs e)
    {
        AttackCallback();
    }

    private void AttackCallback()
    {
        FindFirstObjectByType<CinemachineShake>().ShakeCamera();
    }
    #endregion

    #region Movement
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
        var burst = new ParticleSystem.Burst(0, 7);
        walkParticle.Emit((int)burst.count.constant);
        yield return new WaitForSeconds(0.1f);
        burst.count = 0;
    }

    private void WalkDOTAnim()
    {
        walkDOT.DORestart();
    }
    #endregion
}
