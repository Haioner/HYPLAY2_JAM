using UnityEngine;

public class BossCallbacks : MonoBehaviour
{
    [SerializeField] private ParticleSystem handExplodeParticle;
    [SerializeField] private ParticleSystem handEnemyParticle;
    [SerializeField] private Transform rightHand, leftHand;
    [SerializeField] private GameObject[] orbitalVFX;

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
        if (bossMovement.IsMoving) return;

        anim.SetBool("Path", bossController.isPath);

        bool canEnableOrbital = anim.GetCurrentAnimatorStateInfo(0).IsName("Path");
        if (canEnableOrbital)
        {
            foreach (var item in orbitalVFX)
                item.SetActive(bossController.isPath);
        }
    }

    private void SpawnExplode(object sender, System.EventArgs e)
    {
        anim.SetTrigger("Explode");
    }

    public void SpawnExplodeParticle_EVENT()
    {
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
        FindFirstObjectByType<CinemachineShake>().ShakeCamera();
        Instantiate(handEnemyParticle, rightHand.position, Quaternion.identity, rightHand);
    }
}
