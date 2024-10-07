using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackDelay = 1f;

    [Space][SerializeField] private UnityEvent onAttackStart;

    private float delayTimer;
    private bool canDamage;
    private BoxCollider2D boxCollider;
    private ParticleSystem attackParticle;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        attackParticle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (delayTimer < attackDelay)
            delayTimer += Time.deltaTime;
        else if (!canDamage)
            AttackStart();
    }

    private void AttackStart()
    {
        canDamage = true;
        attackParticle.Play();
        onAttackStart?.Invoke();
        DamageNear();
    }

    private void DamageNear()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamage damageable) && canDamage)
            {
                damageable.Damage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }
}
