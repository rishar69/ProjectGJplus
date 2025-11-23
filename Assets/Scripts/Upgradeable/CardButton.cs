using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro; 

public class CardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Refs")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image iconImage;
    public Button button;

    private CardData assignedCard;
    private System.Action<CardData> onSelected;

    private Vector3 originalScale;

    [Header("Hover Settings")]
    public float hoverScale = 1.08f;
    public float hoverDuration = 0.15f;
    public Ease hoverEase = Ease.OutBack;

    private Tween hoverTween;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        originalScale = transform.localScale;

        if (button != null)
            button.onClick.AddListener(HandleClick);
        else
            Debug.LogWarning($"[CardButton] Tidak menemukan Button di {name}");
    }

    private void OnDisable()
    {
        if (hoverTween != null)
        {
            hoverTween.Kill();
            hoverTween = null;
        }

        transform.localScale = originalScale;
    }

    public void SetCard(CardData data, System.Action<CardData> onSelectedCallback)
    {
        assignedCard = data;
        onSelected = onSelectedCallback;

        if (nameText != null)
            nameText.text = data != null ? data.cardName : "-";

        if (descText != null)
            descText.text = data != null ? data.description : "";

        if (iconImage != null)
            iconImage.sprite = data != null ? data.icon : null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (hoverTween != null) hoverTween.Kill();

        hoverTween = transform
            .DOScale(originalScale * hoverScale, hoverDuration)
            .SetEase(hoverEase)
            .SetUpdate(true); // 🔥 pakai unscaled time, jadi tetap jalan saat Time.timeScale = 0
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (hoverTween != null) hoverTween.Kill();

        hoverTween = transform
            .DOScale(originalScale, hoverDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true); // 🔥 juga pakai unscaled time
    }

    private void HandleClick()
    {
        if (assignedCard == null)
        {
            Debug.LogWarning("[CardButton] Tidak ada CardData yang ter-assign.");
            return;
        }

        // Optional: efek klik
        transform.DOPunchScale(Vector3.one * 0.08f, 0.25f, 6, 0.4f).SetUpdate(true);

        onSelected?.Invoke(assignedCard);
    }
}
