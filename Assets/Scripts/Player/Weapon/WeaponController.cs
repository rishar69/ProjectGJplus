using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    private void Awake()
    {
        // Replace the array of prefabs with actual instances
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1) EquipWeapon(1);

        // Reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
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

    private void StartReload()
    {
        // Start QTE for reload
        QTEManager.Instance.StartQTE(transform, currentWeapon.reloadDuration, (success) =>
        {
            currentWeapon.RefillAmmo(success);
            Debug.Log($"Reload {(success ? "success" : "failed")}. Ammo: {currentWeapon.currentAmmo}");
        });
    }
}
