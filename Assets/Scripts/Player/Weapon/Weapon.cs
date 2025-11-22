using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float fireRate;

    protected float nextFireTime;

    public abstract void Shoot();
}
