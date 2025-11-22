using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float contactDamage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger hit: " + collision.name);

        if (!collision.CompareTag("Player"))
        {
            Debug.Log("Not player, ignoring.");
            return;
        }

        Health h = collision.GetComponentInParent<Health>();

        if (h != null)
        {
            Debug.Log("Damage applied!");
            h.TakeDamage(contactDamage);
        }
        else
        {
            Debug.Log("Player hit but no Health component found.");
        }
    }
}
