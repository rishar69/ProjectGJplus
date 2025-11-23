using UnityEngine;

public class XPTest : MonoBehaviour
{
     public PlayerLevel level;

    public void Add20XP()
    {
        level.AddXP(20);
        Debug.Log("Added 20 XP");
    }
}
