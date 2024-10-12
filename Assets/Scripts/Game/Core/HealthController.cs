using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public event System.EventHandler OnDeath;
    public event System.EventHandler OnDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void AddHealth(float addValue)
    {
        currentHealth += addValue;
    }

    public void Damage(float damageValue)
    {
        OnDamage?.Invoke(this, System.EventArgs.Empty);
        currentHealth -= damageValue;
        Death();
    }

    private void Death()
    {
        if(currentHealth <= 0)
        {
            OnDeath?.Invoke(this, System.EventArgs.Empty);
            OnDeath = null;
            currentHealth = 0;
        }
    }
}
