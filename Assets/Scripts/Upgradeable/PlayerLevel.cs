using UnityEngine;
using System;

public class PlayerLevel : MonoBehaviour
{
       public int level = 1;
    public int currentXP = 0;

    [Tooltip("XP base, total XP per level = level * xpPerLevelBase")]
    public int xpPerLevelBase = 100;

    public event Action<int> OnLevelUp; // int = level baru

    public int XPNeeded => level * xpPerLevelBase;

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= XPNeeded)
        {
            currentXP -= XPNeeded;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        OnLevelUp?.Invoke(level);
        Debug.Log($"LEVEL UP! New Level: {level}");
    }
}