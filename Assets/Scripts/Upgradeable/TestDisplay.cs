using UnityEngine;
using UnityEngine.UI;

public class TestDisplay : MonoBehaviour
{
   public PlayerStats stats;
    public PlayerLevel level;
    public Text statsText;

    void Update()
    {
        statsText.text =
            "LEVEL: " + level.level + "\n" +
            "XP: " + level.currentXP + " / " + level.XPNeeded + "\n\n" +
            "Attack   : " + stats.attack + "\n" +
            "HP       : " + stats.hp + "\n" +
            "Move SPD : " + stats.moveSpeed + "\n" +
            "Ammo     : " + stats.ammo + "\n" +
            "FireRate : " + stats.fireRate + "\n";
    }
}