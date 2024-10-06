using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private string collisionTag = "Player";
    public event System.EventHandler OnTriggerAttack;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(collisionTag))
        {
            collision.gameObject.GetComponent<HealthController>().Damage(damage);
            OnTriggerAttack?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
