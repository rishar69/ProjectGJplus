using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float contactDamage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only hit the player
        if (!collision.CompareTag("Player"))
            return;

        Health playerHealth = collision.GetComponentInParent<Health>();
        Armor playerArmor = collision.GetComponentInParent<Armor>();
        SheildMode shield = collision.GetComponentInParent<SheildMode>();

        float damageAmount = contactDamage;

        // If shield is active, convert damage to 1 for armor only
        if (shield != null && shield.isShielding)
        {
            shield.AbsorbDamage(ref damageAmount); // armor takes 1, health unaffected
        }
        else
        {
            // Not shielding: damage goes straight to health
            if (playerHealth != null)
                playerHealth.TakeDamage(damageAmount);

            // Armor is untouched
            damageAmount = 0f;
        }

        Debug.Log($"Damage applied: {damageAmount}");
    }
}
