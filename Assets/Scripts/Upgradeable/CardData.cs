// CardData.cs
using UnityEngine;

public enum CardEffectType
{
    Stats,
    SkillUnlock,
    SkillUpgrade
}

[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card", order = 1)]
public class CardData : ScriptableObject
{
    [Header("Info")]
    public string cardID;
    public string cardName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Effect Type")]
    public CardEffectType effectType = CardEffectType.Stats;

    [Header("Stat Settings (dipakai kalau EffectType = Stats)")]
    public float attackMultiplier = 1f;
    public float hpMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public int ammoBonus = 0;

    [Space(10)]
    [Header("Skill Settings (dipakai kalau EffectType = SkillUnlock / SkillUpgrade)")]
    public SkillData skillToUnlock;
    public int skillLevelDelta = 1; // kalau temanmu pakai sistem level skill
}
