// CardRewardUI.cs
using UnityEngine;

public class CardRewardUI : MonoBehaviour
{
    [Header("Referensi")]
    public PlayerLevel playerLevel;
    public CardManager cardManager;

    [Header("UI")]
    public GameObject rewardPanel;     // PASTIKAN ini adalah CHILD, CardRewardUI tetap active
    public CardButton[] cardButtons;

    [Header("Card Pool (isi dari ScriptableObject CardData)")]
    public CardData[] cardPool;

    private void Start()
    {
        if (playerLevel == null)
            playerLevel = FindObjectOfType<PlayerLevel>();

        if (cardManager == null)
            cardManager = FindObjectOfType<CardManager>();

        if (playerLevel != null)
        {
            playerLevel.OnLevelUp += HandleLevelUp;
            Debug.Log("[CardRewardUI] Subscribed to PlayerLevel.OnLevelUp");
        }
        else
        {
            Debug.LogError("[CardRewardUI] PlayerLevel tidak ditemukan di scene!");
        }

        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerLevel != null)
        {
            playerLevel.OnLevelUp -= HandleLevelUp;
        }
    }

    private void HandleLevelUp(int newLevel)
    {
        Debug.Log("[CardRewardUI] HandleLevelUp dipanggil, level: " + newLevel);
        ShowCardChoices();
    }

    private void ShowCardChoices()
    {
        if (cardPool == null || cardPool.Length == 0)
        {
            Debug.LogWarning("CardRewardUI: cardPool kosong");
            return;
        }

        if (rewardPanel == null)
        {
            Debug.LogError("CardRewardUI: rewardPanel belum di-assign!");
            return;
        }

        rewardPanel.SetActive(true);
        Debug.Log("[CardRewardUI] Menampilkan rewardPanel, generate kartu");

        for (int i = 0; i < cardButtons.Length; i++)
        {
            CardButton btn = cardButtons[i];
            if (btn == null) continue;

            CardData randomCard = cardPool[Random.Range(0, cardPool.Length)];

            btn.SetCard(randomCard, (chosenCard) =>
            {
                Debug.Log("[CardRewardUI] Kartu dipilih: " + chosenCard.cardName);

                cardManager.ApplyCard(chosenCard);
                rewardPanel.SetActive(false);

                // lanjut jalan lagi
                Time.timeScale = 1f;
            });
        }

        // pause game saat milih kartu, tapi UI tetap bisa jalan (DOTween kita nanti ignore timescale)
        Time.timeScale = 0f;
    }

    public void ClosePanelWithoutChoosing()
    {
        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
