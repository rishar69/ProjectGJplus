using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public UpgradeCard[] allCards;

    private void Awake()
    {
        Instance = this;
    }

    public void GiveRandomUpgrades()
    {
        if (allCards.Length < 2)
        {
            Debug.LogError("Not enough UpgradeCards in list!");
            return;
        }

        // pick 2 random different cards
        UpgradeCard card1;
        UpgradeCard card2;

        card1 = allCards[Random.Range(0, allCards.Length)];

        do
        {
            card2 = allCards[Random.Range(0, allCards.Length)];
        }
        while (card2 == card1);

        // Show UI
        LevelUpUI.Instance.Show(card1, card2);
    }
}
