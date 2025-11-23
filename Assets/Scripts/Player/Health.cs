using UnityEngine;

using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public float HealthPercent => CurrentHealth / MaxHealth;

    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;  
    public UnityEvent OnDeath;

    private bool isDead = false;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        Debug.Log("HealthChanged event fired: " + HealthPercent);

        OnHealthChanged?.Invoke(HealthPercent);
        if (CurrentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(HealthPercent);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        CurrentHealth += amount; // give bonus heal
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(HealthPercent);
    }
}
