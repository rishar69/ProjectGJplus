using UnityEngine;

public class ArmorTester : MonoBehaviour
{
    public float amount = 10f;
    private Armor armor;

    private void Awake()
    {
        armor = GetComponent<Armor>();
        if (armor == null)
            Debug.LogError("Armor component not found on this GameObject!");
    }

    private void Update()
    {
        // Press space to reduce armor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Testing armor damage: -{amount}");
            armor.TakeDamage(amount);
        }

        // Press H to restore armor
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"Restoring armor: +{amount}");
            armor.Restore(amount);
        }
    }
}
