using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Instance;

    [Header("UI")]
    public CanvasGroup panel;

    public UpgradeCardDisplay card1;
    public UpgradeCardDisplay card2;

    private UpgradeCard selected1;
    private UpgradeCard selected2;

    private GameObject player;

    private void Awake()
    {
        Instance = this;
        panel.alpha = 0;
        panel.blocksRaycasts = false;
        panel.interactable = false;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void Show(UpgradeCard c1, UpgradeCard c2)
    {
        selected1 = c1;
        selected2 = c2;

        card1.Setup(c1, () => Pick(c1));
        card2.Setup(c2, () => Pick(c2));

        panel.alpha = 1;
        panel.blocksRaycasts = true;
        panel.interactable = true;

        Time.timeScale = 1f; // pause game during selection
    }

    public void Pick(UpgradeCard card)
    {
        Debug.Log("Picked upgrade: " + card.name);
        card.ApplyToPlayer(player);

        // Close UI
        panel.alpha = 0;
        panel.blocksRaycasts = false;
        panel.interactable = false;
        Time.timeScale = 1f;
    }
}
