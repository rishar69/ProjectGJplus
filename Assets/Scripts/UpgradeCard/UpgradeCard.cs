using UnityEngine;

public enum UpgradeType
{
    Damage,
    MaxHealth,
    MoveSpeed,
    MaxAmmo,
    Armor
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade Card")]
public class UpgradeCard : ScriptableObject
{
    public string cardName;
    public Sprite icon;
    public UpgradeType type;
    public float amount;   // how much upgrade increases

    public void ApplyToPlayer(GameObject player)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                ApplyDamage(player);
                break;

            case UpgradeType.MaxHealth:
                ApplyMaxHealth(player);
                break;

            case UpgradeType.MoveSpeed:
                ApplyMoveSpeed(player);
                break;

            case UpgradeType.MaxAmmo:
                ApplyMaxAmmo(player);
                break;

            case UpgradeType.Armor:
                ApplyArmor(player);
                break;
        }
    }

    private void ApplyDamage(GameObject player)
    {
        Weapon weapon = player.GetComponentInChildren<Weapon>();
        if (weapon != null)
            weapon.damage += amount;
    }

    private void ApplyMaxHealth(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (health != null)
        {
            // increase max
            var max = health.MaxHealth + amount;

            // heal by amount
            var heal = amount;

            // apply
            SetPrivateField(health, "maxHealth", max);
            health.Heal(heal);
        }
    }

    private void ApplyMoveSpeed(GameObject player)
    {
        PlayerMovement move = player.GetComponent<PlayerMovement>();
        if (move != null)
            move.moveSpeed += amount;
    }

    private void ApplyMaxAmmo(GameObject player)
    {
        Weapon weapon = player.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.magazineSize += Mathf.RoundToInt(amount);
            weapon.currentAmmo = weapon.magazineSize; // refill to new max
        }
    }

    private void ApplyArmor(GameObject player)
    {
        Armor armor = player.GetComponent<Armor>();
        if (armor != null)
        {
            float newMax = armor.MaxArmor + amount;
            SetPrivateField(armor, "maxArmor", newMax);
            armor.Restore(amount);
        }
    }

    // utility to modify private serialized fields
    private void SetPrivateField(object obj, string field, float value)
    {
        var type = obj.GetType();
        var f = type.GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (f != null)
            f.SetValue(obj, value);
    }
}
