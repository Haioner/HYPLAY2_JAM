using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField] private float damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            collision.gameObject.GetComponent<HealthController>().Damage(damage);
    }
}
