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
        // Toggle shield mode
        if (Input.GetKeyDown(shieldKey))
        {
            ActivateShield();
        }
        else if (Input.GetKeyUp(shieldKey))
        {
            DeactivateShield();
        }
    }

    private void ActivateShield()
    {
        isShielding = true;
        movement.isShielding = true; // Prevent input but keep velocity
        Debug.Log("Shield activated");
    }

    private void DeactivateShield()
    {
        isShielding = false;
        movement.isShielding = false; // Resume normal input
        Debug.Log("Shield deactivated");
    }

    /// <summary>
    /// Call this when player is about to take damage
    /// </summary>
    public void AbsorbDamage(ref float damage)
    {
        if (!isShielding) return;

        if (damage > 0)
        {
            armor.TakeDamage(1f);
            damage = 0f;
            Debug.Log("Shield absorbed damage. Health unaffected, 1 damage to armor.");
        }
    }
}
