using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    private Armor armor;
    private Slider slider;

    [Header("Smooth Settings")]
    public float smoothSpeed = 10f;

    private float targetValue = 1f;

    private void Awake()
    {
        armor = GetComponentInParent<Armor>();
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;

        targetValue = armor.ArmorPercent;
        slider.value = armor.ArmorPercent;

        armor.OnArmorChanged.AddListener(UpdateArmorBar);
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, targetValue, smoothSpeed * Time.deltaTime);
    }

    private void UpdateArmorBar(float percent)
    {
        targetValue = percent;
    }
}
