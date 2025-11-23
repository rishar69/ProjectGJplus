using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;

    [Header("Tween Settings")]
    public float hoverScale = 1.1f;
    public float hoverDuration = 0.15f;
    public float clickScale = 0.9f;
    public float clickDuration = 0.1f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, hoverDuration)
            .SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, hoverDuration)
            .SetEase(Ease.OutBack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // scale kecil → balik ke besar
        transform.DOScale(originalScale * clickScale, clickDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                transform.DOScale(originalScale, clickDuration)
                    .SetEase(Ease.OutBack);
            });
    }
}
