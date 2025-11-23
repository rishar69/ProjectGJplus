using UnityEngine;
using FMODUnity;

public class Handgun : Weapon
{
    public Bullet bulletPrefab;
    public Transform firePoint;

    [Header("Muzzle Flash Animation")]
    public Animator muzzleAnimator;
    public string shootTriggerName = "Shoot";

    [Header("Audio")]
    public EventReference handgunShotEvent;   // drag event SFX/HandgunShot di inspector

    [Header("Camera Recoil")]
    public float shakeIntensity = 7f;      // makin besar makin liar
    public float shakeDuration = 0.18f;

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

        // Spawn bullet
        Bullet b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.damage = damage;
        b.SetDirection(firePoint.right);

        // 🔥 Trigger muzzle flash animation
        if (muzzleAnimator != null)
        {
            muzzleAnimator.SetTrigger(shootTriggerName);
        }

        // 🔊 FMOD handgun shot
        if (!handgunShotEvent.IsNull)
        {
            AudioManager.Instance.PlayOneShot(handgunShotEvent, firePoint.position);
        }

      if (CameraShake.Instance != null)
{
    CameraShake.Instance.ShakeExtreme();       // shake brutal
    CameraShake.Instance.RecoilKickExtreme(); // recoil brutal
}
        // Ammo
        ConsumeAmmo();
        nextFireTime = Time.time + 1f / fireRate;
    }
}
