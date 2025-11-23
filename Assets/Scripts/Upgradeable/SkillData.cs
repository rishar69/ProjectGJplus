using UnityEngine;

public enum SkillType
{
    Active,   // ditekan (Q/E/R)
    Passive   // buff pasif
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Game/Skill", order = 2)]
public class SkillData : ScriptableObject
{
    [Header("Info")]
    public string skillID;
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;
    public SkillType type = SkillType.Active;

    [Header("Cooldown & Cost")]
    public float cooldown = 5f;
    public int manaCost = 0;  // kalau belum pakai mana, cuekin aja

    [Header("Scaling dari PlayerStats")]
    public float attackMultiplier = 1f;  // damage = stats.attack * ini
    public float hpMultiplier = 0f;      // buat heal/shield kalau perlu

    [Header("Lainnya")]
    public float range = 5f;
    public float duration = 0f;
}
