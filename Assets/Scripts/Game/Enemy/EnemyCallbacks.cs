using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCallbacks : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private DOTweenAnimation walkDOT;
    EnemyMovement enemyMovement;

    [Header("Attack")]
    [SerializeField] private MeeleAttack meeleAttack;

    [Header("Death")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private AudioClip deathClip;

    private void OnEnable()
    {
        healthController = GetComponent<HealthController>();
        enemyMovement = GetComponent<EnemyMovement>();

        enemyMovement.OnStartMove += StartMoveCallback;
        meeleAttack.OnTriggerAttack += StartAttackCallback;
        healthController.OnDeath += DeathCallback;
    }

    private void OnDisable()
    {
        enemyMovement.OnStartMove -= StartMoveCallback;
        meeleAttack.OnTriggerAttack -= StartAttackCallback;
        healthController.OnDeath -= DeathCallback;
    }

    private void DeathCallback(object sender, System.EventArgs e)
    {
        SoundManager.PlayAudioClip(deathClip);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject.transform.parent.gameObject);
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
