using UnityEngine;
using System;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;

    [Header("Base XP for Level 1 - 2")]
    public float baseXP = 100f;

    [Header("XP Scaling Curve (x = level, y = multiplier)")]
    public AnimationCurve xpCurve;

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPChanged; // (currentXP, XPNeeded)

    private float currentLevelXPNeeded;

    private void Start()
    {
        currentLevelXPNeeded = baseXP;

        // Trigger initial UI update
        OnXPChanged?.Invoke(currentXP, XPNeeded);
        OnLevelUp?.Invoke(level);
    }

    public int XPNeeded => Mathf.CeilToInt(currentLevelXPNeeded);

    public void AddXP(int amount)
    {
        currentXP += amount;

        // Fire event
        OnXPChanged?.Invoke(currentXP, XPNeeded);

        while (currentXP >= XPNeeded)
        {
            currentXP -= XPNeeded;
            LevelUp();

            // Fire XP changed after reducing
            OnXPChanged?.Invoke(currentXP, XPNeeded);
        }
    }

    private void LevelUp()
    {
        level++;

        ApplyCurveScaling();

        OnLevelUp?.Invoke(level);
        Debug.Log($"LEVEL UP! New Level: {level}, Next XP Needed: {XPNeeded}");
    }

    private void ApplyCurveScaling()
    {
        float multiplier = xpCurve.Evaluate(level);

        // Prevent 0 or negative
        multiplier = Mathf.Max(1f, multiplier);

        currentLevelXPNeeded *= multiplier;
    }
}
