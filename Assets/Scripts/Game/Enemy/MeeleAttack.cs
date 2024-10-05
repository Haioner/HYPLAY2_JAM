using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private string collisionTag = "Player";
    public event System.EventHandler OnTriggerAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(collisionTag))
        { 
            collision.gameObject.GetComponent<HealthController>().Damage(damage);
            OnTriggerAttack?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
