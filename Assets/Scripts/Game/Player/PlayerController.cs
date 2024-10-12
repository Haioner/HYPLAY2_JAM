using UnityEngine.UI;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5.0f;

    [Header("Shield")]
    [SerializeField] private HealthController playerHealth;
    [SerializeField] private GameObject shield;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private float shieldMaxValue;
    [SerializeField] private MMF_Player removeShieldFeedbacks;
    [SerializeField] private MMF_Player addShieldFeedbacks;
    private float currentShieldValue;
    private bool haveShield;

    private void OnEnable()
    {
        playerHealth.OnDamage += RemoveShield;
    }

    private void OnDisable()
    {
        playerHealth.OnDamage -= RemoveShield;
    }

    private void Awake()
    {
        shieldSlider.maxValue = shieldMaxValue;
    }

    public void AddShield(float addValue)
    {
        if (currentShieldValue < shieldMaxValue)
            currentShieldValue += addValue;

        if (currentShieldValue >= shieldMaxValue && !haveShield)
        {
            haveShield = true;
            addShieldFeedbacks.PlayFeedbacks();
            playerHealth.AddHealth(1);
        }

        shieldSlider.value = currentShieldValue;
    }

    private void RemoveShield(object sender, System.EventArgs e)
    {
        if (haveShield)
        {
            haveShield = false;
            removeShieldFeedbacks.PlayFeedbacks();
            currentShieldValue = 0;
            shieldSlider.value = 0;
        }
    }
}
