using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    private bool isReloading = false;

    private void Awake()
    {
        // Instantiate weapon instances as children of the player
        for (int i = 0; i < weapons.Length; i++)
        {
            Weapon instance = Instantiate(weapons[i], transform);
            instance.gameObject.SetActive(false);
            weapons[i] = instance;
        }

        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        // Weapon switch
        if (!isReloading)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1) EquipWeapon(1);
        }

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }

        // Auto-reload if ammo is empty
        if (!isReloading && currentWeapon != null && currentWeapon.currentAmmo <= 0)
        {
            TryReload();
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        currentWeaponIndex = index;
        currentWeapon = weapons[index];
        currentWeapon.gameObject.SetActive(true);
    }

    private void TryReload()
    {
        if (currentWeapon == null || isReloading || currentWeapon.currentAmmo >= currentWeapon.magazineSize)
            return;

        isReloading = true;

        // Start QTE for reload
        QTEManager.Instance.StartQTE(transform, currentWeapon.reloadDuration, (success) =>
        {
            currentWeapon.RefillAmmo(success);
            Debug.Log($"Reload {(success ? "success" : "failed")}. Ammo: {currentWeapon.currentAmmo}");
            isReloading = false; // allow future reloads
        });
    }
}
