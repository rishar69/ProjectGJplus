using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardDisplay : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text description;
    public Button pickButton;

    public void Setup(UpgradeCard card, UnityEngine.Events.UnityAction onClick)
    {
        icon.sprite = card.icon;
        title.text = card.cardName;
        description.text = $"+{card.amount} {card.type}";

        pickButton.onClick.RemoveAllListeners();
        pickButton.onClick.AddListener(onClick);
    }
}
