using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public Transform weaponParent;
    public Weapon currentWeapon;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // Delete old weapon
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Spawn new weapon
        GameObject newWeapon = Instantiate(weaponPrefab, weaponParent.position, weaponParent.rotation, weaponParent);
        currentWeapon = newWeapon.GetComponent<Weapon>();
    }
}
