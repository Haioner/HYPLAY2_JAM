using UnityEngine;
using UnityEngine.Events;

public class EnemyDanger : MonoBehaviour
{
    [SerializeField] private float attackDelay = 1f;

    [Space][SerializeField] private UnityEvent onAttackStart;
    private float delayTimer;
    private bool hasSpawned;

    private void Update()
    {
        if (hasSpawned) return;

        if (delayTimer < attackDelay)
            delayTimer += Time.deltaTime;
        else
        {
            onAttackStart?.Invoke();
            hasSpawned = true;
        }
    }


}
