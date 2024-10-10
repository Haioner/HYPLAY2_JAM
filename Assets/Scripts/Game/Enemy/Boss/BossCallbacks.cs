using MoreMountains.Feedbacks;
using UnityEngine;

public class BossCallbacks : MonoBehaviour
{
    [SerializeField] private ParticleSystem handExplodeParticle;
    [SerializeField] private ParticleSystem handEnemyParticle;
    [SerializeField] private Transform rightHand, leftHand;
    [SerializeField] private GameObject[] orbitalVFX;
    [SerializeField] private MMF_Player explodeFEEDBACK;
    [SerializeField] private MMF_Player enemyFEEDBACK;

    private BossMovement bossMovement;
    private BossController bossController;
    private Animator anim;

    private void Start()
    {
        bossMovement = GetComponent<BossMovement>();
        bossController = GetComponent<BossController>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        BossController.OnExplodeSpawn += SpawnExplode;
        BossController.OnEnemySpawn += SpawnEnemy;
    }

    private void OnDisable()
    {
        BossController.OnExplodeSpawn -= SpawnExplode;
        BossController.OnEnemySpawn -= SpawnEnemy;

    }

    private void Update()
    {
        IsPath();
    }

    private void IsPath()
    {
        anim.SetBool("Path", bossController.isPath);

        bool canOrbital = bossController.isPath && !bossMovement.IsMoving;
        foreach (var item in orbitalVFX)
            item.SetActive(canOrbital);
    }

    private void SpawnExplode(object sender, System.EventArgs e)
    {
        anim.SetTrigger("Explode");
    }

    public void SpawnExplodeParticle_EVENT()
    {
        explodeFEEDBACK.PlayFeedbacks();
        FindFirstObjectByType<CinemachineShake>().ShakeCamera();
        Instantiate(handExplodeParticle, rightHand.position, Quaternion.identity, rightHand);
        Instantiate(handExplodeParticle, leftHand.position, Quaternion.identity, leftHand);
    }

    private void SpawnEnemy(object sender, System.EventArgs e)
    {
        anim.SetTrigger("Enemy");
    }

    public void SpawnEnemyParticle_EVENT()
    {
        enemyFEEDBACK.PlayFeedbacks();
        FindFirstObjectByType<CinemachineShake>().ShakeCamera();
        Instantiate(handEnemyParticle, rightHand.position, Quaternion.identity, rightHand);
    }
}
