using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCallbacks : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private DOTweenAnimation walkDOT;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private Vector2 clipPitch = new Vector2(0.9f, 1.2f);

    [Header("Death")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float pitchDecreaseSpeed = 2f;
    [SerializeField] private HealthController healthController;
    [SerializeField] private MMF_Player deathFeedbacks;
    [SerializeField] private Camera mainCamera;
    private bool isDead;
    private float currentPitch = 1;

    private void OnEnable()
    {
        audioMixer.SetFloat("MasterPitch", 1f);
        MovementController.OnStartMove += StartMoveCallback;
        healthController.OnDeath += DeathCallback;
    }

    private void OnDisable()
    {
        MovementController.OnStartMove -= StartMoveCallback;
        healthController.OnDeath -= DeathCallback;
    }

    private void Update()
    {
        if (isDead)
        {
            currentPitch = Mathf.Max(currentPitch - pitchDecreaseSpeed * Time.deltaTime, 0.5f);
            audioMixer.SetFloat("MasterPitch", currentPitch);

            Vector3 targetPosition = transform.position;
            targetPosition.z = -10;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, 4 * Time.deltaTime);
        }
    }

    private void DeathCallback(object sender, System.EventArgs e)
    {
        deathFeedbacks.PlayFeedbacks();
        isDead = true;
    }

    private void StartMoveCallback(object sender, System.EventArgs e)
    {
        SoundManager.PlayContinousAudioClipVolumeAndRandomPitch(walkClip, 1, clipPitch);
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
