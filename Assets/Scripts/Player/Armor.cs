using UnityEngine;
using UnityEngine.Events;

public class Armor : MonoBehaviour
{
    [Header("Armor Settings")]
    [SerializeField] private float maxArmor = 50f;
    public float MaxArmor => maxArmor;
    public float CurrentArmor { get; private set; }
    public float ArmorPercent => CurrentArmor / MaxArmor;

    [Header("Events")]
    public UnityEvent<float> OnArmorChanged;  // Fires with percentage (0-1)
    public UnityEvent OnArmorBroken;          // Fires when armor reaches 0

    private bool isBroken = false;

    private void Awake()
    {
        CurrentArmor = maxArmor;
    }

    /// <summary>
    /// Reduces armor by amount.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (isBroken) return;

        CurrentArmor -= amount;
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0, maxArmor);
        Debug.Log("ArmorChanged event fired: " + ArmorPercent);

        OnArmorChanged?.Invoke(ArmorPercent);

        if (CurrentArmor <= 0)
            BreakArmor();
    }

    /// <summary>
    /// Restores armor by amount.
    /// </summary>
    public void Restore(float amount)
    {
        if (isBroken) return;

        CurrentArmor += amount;
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0, maxArmor);

        OnArmorChanged?.Invoke(ArmorPercent);
    }

    private void BreakArmor()
    {
        if (isBroken) return;

        isBroken = true;
        OnArmorBroken?.Invoke();
        Debug.Log("Armor broken!");
    }

    public void IncreaseMaxArmor(float amount)
    {
        maxArmor += amount;
        CurrentArmor += amount;

        CurrentArmor = Mathf.Clamp(CurrentArmor, 0, maxArmor);

        OnArmorChanged?.Invoke(ArmorPercent);
    }
}
