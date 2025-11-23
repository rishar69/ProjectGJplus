// PlayerStats.cs
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats (sebelum kartu)")]
    public float baseAttack = 10f;
    public float baseHP = 100f;
    public float baseMoveSpeed = 5f;
    public int baseAmmo = 30;
    public float baseFireRate = 2f; // peluru per detik

    [Header("Runtime Stats (pakai di script lain)")]
    public float attack;
    public float hp;
    public float moveSpeed;
    public int ammo;
    public float fireRate;

    private void Awake()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        attack = baseAttack;
        hp = baseHP;
        moveSpeed = baseMoveSpeed;
        ammo = baseAmmo;
        fireRate = baseFireRate;
    }
}
