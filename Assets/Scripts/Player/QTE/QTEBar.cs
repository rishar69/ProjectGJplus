using UnityEngine;
using UnityEngine.UI;
using System;

public class QTEBar : MonoBehaviour
{
    [Header("UI Components")]
    public Slider slider;

    [Header("QTE Settings")]
    [Tooltip("Define success zones as percentages of the slider (0 = left, 1 = right)")]
    public SuccessZone[] successZones;
    public float speedMultiplier = 2f;

    [HideInInspector] public bool inputReceived = false;
    [HideInInspector] public bool success = false;

    private float duration;
    private Action<bool> callback;
    private Transform player;


    [Serializable]
    public struct SuccessZone
    {
        [Range(0f, 1f)] public float start; // Slider 0-1
        [Range(0f, 1f)] public float end;   // Slider 0-1
    }

    /// <summary>
    /// Starts the QTE bar logic
    /// </summary>
    /// <param name="duration">Duration of the QTE in seconds (unscaled time)</param>
    /// <param name="onComplete">Callback: true = success, false = fail</param>
    public void StartQTE(float duration, Action<bool> onComplete)
    {
        this.duration = duration / speedMultiplier; callback = onComplete;

        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;

        inputReceived = false;
        success = false;
    }

    private void Update()
    {
        if (inputReceived) return;

        // Follow player position
        if (player != null)
        {
            transform.position = player.position + Vector3.up * 2f;
        }

        // Advance slider based on unscaled time
        slider.value += Time.unscaledDeltaTime / duration;

        // If player presses the key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputReceived = true;
            success = CheckSuccess();
            Debug.Log("QTE Input Received! Success? " + success);
            callback?.Invoke(success);
        }

        // If slider reaches the end without input
        if (slider.value >= 1f && !inputReceived)
        {
            inputReceived = true;
            success = false;
            Debug.Log("QTE Failed: Slider reached end");
            callback?.Invoke(success);
        }
    }


    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }


    /// <summary>
    /// Checks if slider is inside any success zone
    /// </summary>
    /// <returns></returns>
    private bool CheckSuccess()
    {
        foreach (var zone in successZones)
        {
            if (slider.value >= zone.start && slider.value <= zone.end)
            {
                Debug.Log("QTE CheckSuccess: True for zone " + zone.start + "-" + zone.end);
                return true;
            }
            else
            {
                Debug.Log("QTE CheckSuccess: False for zone " + zone.start + "-" + zone.end);
            }
        }
        return false;
    }
}
