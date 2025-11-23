// CardManager.cs
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerStats))]
public class CardManager : MonoBehaviour
{
    public PlayerStats stats;

    // Event buat ngasi tahu kalo kartu Skill diterapkan
    public event Action<SkillData, CardData> OnSkillCardApplied;

    private void Reset()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void ApplyCard(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("[CardManager] Card null");
            return;
        }

        switch (card.effectType)
        {
            case CardEffectType.Stats:
                ApplyStatCard(card);
                break;

            case CardEffectType.SkillUnlock:
            case CardEffectType.SkillUpgrade:
                HandleSkillCard(card);
                break;
        }

        Debug.Log($"[CardManager] Card applied: {card.cardName} ({card.effectType})");
    }

    private void ApplyStatCard(CardData card)
    {
        if (stats == null) return;

        stats.attack    *= card.attackMultiplier;
        stats.hp        *= card.hpMultiplier;
        stats.moveSpeed *= card.moveSpeedMultiplier;
        stats.fireRate  *= card.fireRateMultiplier;
        stats.ammo      += card.ammoBonus;
    }

    private void HandleSkillCard(CardData card)
    {
        if (card.skillToUnlock == null)
        {
            Debug.LogWarning("[CardManager] Skill card tapi SkillData kosong");
            return;
        }

        // Untuk kartu Skill ini, kita cuma perlu kasih tahu listener-nya.
       
        OnSkillCardApplied?.Invoke(card.skillToUnlock, card);
    }
}
