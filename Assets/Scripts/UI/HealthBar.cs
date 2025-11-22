using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health;
    private Slider slider;

    [Header("Smooth Settings")]
    public float smoothSpeed = 10f;

    private float targetValue = 1f;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;

        targetValue = health.HealthPercent;
        slider.value = health.HealthPercent;

        health.OnHealthChanged.AddListener(UpdateHealthBar);
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, targetValue, smoothSpeed * Time.deltaTime);
    }

    private void UpdateHealthBar(float percent)
    {
        targetValue = percent; 
    }
}
