using UnityEngine;

public class SheildMode : MonoBehaviour
{
    [Header("Shield Settings")]
    public KeyCode shieldKey = KeyCode.Mouse1;
    public bool isShielding { get; private set; } = false;

    private Health health;
    private Armor armor;
    private PlayerMovement movement;

    private void Awake()
    {
        health = GetComponent<Health>();
        armor = GetComponent<Armor>();
        movement = GetComponent<PlayerMovement>();

        if (!health || !armor || !movement)
            Debug.LogError("Missing Health, Armor, or PlayerMovement component!");
    }

    private void Update()
    {
        // Prevent shield if armor is gone
        if (armor.CurrentArmor <= 0 && isShielding)
        {
            DeactivateShield();
        }

        // Shield activation
        if (Input.GetKeyDown(shieldKey))
        {
            TryActivateShield();
        }
        else if (Input.GetKeyUp(shieldKey))
        {
            DeactivateShield();
        }
    }

    private void TryActivateShield()
    {
        if (armor.CurrentArmor <= 0)
        {
            Debug.Log("Cannot activate shield. Armor depleted!");
            return;
        }

        ActivateShield();
    }

    private void ActivateShield()
    {
        isShielding = true;
        movement.isShielding = true;
        Debug.Log("Shield activated");
    }

    private void DeactivateShield()
    {
        isShielding = false;
        movement.isShielding = false;
        Debug.Log("Shield deactivated");
    }

    /// <summary>
    /// Called before taking damage
    /// </summary>
    public void AbsorbDamage(ref float damage)
    {
        if (!isShielding) return;

        // If armor breaks while shielding
        if (armor.CurrentArmor <= 0)
        {
            DeactivateShield();
            return;
        }

        if (damage > 0)
        {
            armor.TakeDamage(1f);   // reduce armor by 1
            damage = 0f;           // block health damage
            Debug.Log("Shield absorbed damage.");
        }
    }
}
