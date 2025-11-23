using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName;
    public int magazineSize = 10;
    public int currentAmmo;
    public float damage = 10f;
    public float fireRate = 2f; // shots per second
    public float reloadDuration = 3f;
    public Sprite WeaponIcon;


    protected float nextFireTime;

    public abstract void Shoot();

    public bool CanShoot()
    {
        return Time.time >= nextFireTime && currentAmmo > 0;
    }

    public void ConsumeAmmo()
    {
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
    }

    public bool IsEmpty()
    {
        return currentAmmo <= 0;
    }

    public void RefillAmmo(bool full)
    {
        currentAmmo = full ? magazineSize : magazineSize / 2;
    }

    public void IncreaseDamage(float amount)
    {
        damage += amount;
    }

    public void IncreaseMagazine(int amount)
    {
        magazineSize += amount;
        currentAmmo = magazineSize; // refill since magazine size changed
    }
}
