using UnityEngine;

public class MachineGun : Weapon
{
    public Bullet bulletPrefab;
    public Transform firePoint;

    private void Start()
    {
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && CanShoot())
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;

        Bullet b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.damage = damage; // set bullet damage per weapon
        b.SetDirection(firePoint.right);

        ConsumeAmmo();
        nextFireTime = Time.time + 1f / fireRate;
    }
}
