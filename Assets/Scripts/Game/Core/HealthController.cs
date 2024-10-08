using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public event System.EventHandler OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Damage(float damageValue)
    {
        currentHealth -= damageValue;
        Death();
    }

    private void Death()
    {
        if(currentHealth <= 0)
        {
            OnDeath?.Invoke(this, System.EventArgs.Empty);
            currentHealth = 0;
        }
    }
}
