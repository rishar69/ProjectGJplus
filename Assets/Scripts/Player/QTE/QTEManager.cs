using UnityEngine;
using System.Collections;
using System;
using Unity.Cinemachine; // CM 3.x

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("QTE Settings")]
    public QTEBar qteBarPrefab;
    public float qteDuration = 3f;

    [Header("Camera (Cinemachine 3.x) Settings")]
    public CinemachineCamera virtualCamera;      // <== pakai CinemachineCamera (CM 3.x)
    public float cameraZoomAmount = 3f;
    public float cameraZoomSpeed = 5f;

    private float originalCameraSize;
    private Coroutine qteCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // auto find CinemachineCamera kalau lupa assign di Inspector
        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineCamera>();

        if (virtualCamera != null)
        {
            // 2D: kamera utama sudah orthographic, cukup simpan Orthographic Size
            originalCameraSize = virtualCamera.Lens.OrthographicSize;
        }
        else
        {
            Debug.LogError("[QTEManager] CinemachineCamera tidak ditemukan!");
        }
    }

    public void StartQTE(Transform player, float duration, Action<bool> onComplete)
    {
        if (qteCoroutine != null) StopCoroutine(qteCoroutine);
        qteCoroutine = StartCoroutine(QTERoutine(player, duration, onComplete));
    }

    private IEnumerator QTERoutine(Transform player, float duration, Action<bool> onComplete)
    {
        if (virtualCamera == null)
        {
            Debug.LogError("[QTEManager] Kamera kosong, zoom QTE tidak berjalan.");
        }

        // ---------- ZOOM IN ----------
        if (virtualCamera != null)
        {
            float startSize = virtualCamera.Lens.OrthographicSize;
            originalCameraSize = startSize; // backup

            float targetSize = Mathf.Max(0.1f, startSize - cameraZoomAmount);

            while (Mathf.Abs(virtualCamera.Lens.OrthographicSize - targetSize) > 0.01f)
            {
                float newSize = Mathf.Lerp(
                    virtualCamera.Lens.OrthographicSize,
                    targetSize,
                    cameraZoomSpeed * Time.unscaledDeltaTime
                );

                var lens = virtualCamera.Lens;
                lens.OrthographicSize = newSize;
                virtualCamera.Lens = lens;

                yield return null;
            }

            var finalLensIn = virtualCamera.Lens;
            finalLensIn.OrthographicSize = targetSize;
            virtualCamera.Lens = finalLensIn;
        }

        // ---------- SPAWN QTE ----------
        QTEBar bar = Instantiate(qteBarPrefab, player.position + Vector3.up * 2f, Quaternion.identity);
        bar.SetPlayer(player);
        bar.StartQTE(duration, success => onComplete?.Invoke(success));

        while (!bar.inputReceived)
            yield return null;

        Destroy(bar.gameObject);

        // ---------- ZOOM OUT ----------
        if (virtualCamera != null)
        {
            while (Mathf.Abs(virtualCamera.Lens.OrthographicSize - originalCameraSize) > 0.01f)
            {
                float newSize = Mathf.Lerp(
                    virtualCamera.Lens.OrthographicSize,
                    originalCameraSize,
                    cameraZoomSpeed * Time.unscaledDeltaTime
                );

                var lens = virtualCamera.Lens;
                lens.OrthographicSize = newSize;
                virtualCamera.Lens = lens;

                yield return null;
            }

            var finalLensOut = virtualCamera.Lens;
            finalLensOut.OrthographicSize = originalCameraSize;
            virtualCamera.Lens = finalLensOut;
        }

        qteCoroutine = null;
    }
}
