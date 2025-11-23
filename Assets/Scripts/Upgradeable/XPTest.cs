using UnityEngine;

public class PlayerLevelTester : MonoBehaviour
{
    public PlayerLevel playerLevel;

    [Header("XP Gain Settings")]
    public int xpPerPress = 50;

    private void Update()
    {
        // Press T to simulate XP gain
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerLevel.AddXP(xpPerPress);
            Debug.Log($"Added {xpPerPress} XP | Current XP: {playerLevel.currentXP}/{playerLevel.XPNeeded}");
        }
    }
}
