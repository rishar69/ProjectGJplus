using UnityEngine;
using System.Collections;
using System;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("QTE Settings")]
    public QTEBar qteBarPrefab;
    public float qteDuration = 3f;
    public float cameraZoomAmount = 3f; // Orthographic size decrease
    public float cameraZoomSpeed = 5f;

    private Camera mainCamera;
    private float originalCameraSize;
    private Coroutine qteCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    public void StartQTE(Transform player, float duration, Action<bool> onComplete)
    {
        if (qteCoroutine != null) StopCoroutine(qteCoroutine);
        qteCoroutine = StartCoroutine(QTERoutine(player, duration, onComplete));
    }

    private IEnumerator QTERoutine(Transform player, float duration, Action<bool> onComplete)
    {
        // Save camera size
        originalCameraSize = mainCamera.orthographicSize;

        // Camera zoom in
        float targetSize = originalCameraSize - cameraZoomAmount;
        while (Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, cameraZoomSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;

        // Spawn QTEBar
        QTEBar bar = Instantiate(qteBarPrefab, player.position + Vector3.up * 2f, Quaternion.identity);
        bar.SetPlayer(player); // follow player
        bar.StartQTE(duration, (success) => { onComplete?.Invoke(success); });

        // Wait until QTE finishes
        while (!bar.inputReceived)
            yield return null;

        Destroy(bar.gameObject);

        // Restore camera
        while (Mathf.Abs(mainCamera.orthographicSize - originalCameraSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalCameraSize, cameraZoomSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        mainCamera.orthographicSize = originalCameraSize;

        qteCoroutine = null;
    }
}
