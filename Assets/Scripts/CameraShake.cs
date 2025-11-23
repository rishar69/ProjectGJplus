using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Noise / Shake Settings (EXTREME)")]
    public float extremeIntensity = 10f;       // default = 10, bisa 15–20
    public float extremeDuration = 0.25f;      // durasi lebih lama
    public float baseAmplitude = 0f;
    public float baseFrequency = 1f;

    [Header("Recoil Kick (EXTREME)")]
    public Transform recoilTarget;
    public float recoilDistance = 0.15f;        // makin besar makin brutal
    public float recoilRandom = 0.1f;           // acakan recoil samping
    public float recoilReturnSpeed = 12f;

    [Header("Camera Tilt (Bonus)")]
    public float tiltAmount = 3f;               // derajat tilt kiri/kanan
    public float tiltSpeed = 8f;

    private CinemachineVirtualCameraBase vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;
    private float shakeDuration;
    private float targetAmplitude;

    private float currentTilt = 0f;
    private float tiltVelocity = 0f;

    private Vector3 recoilOriginalPos;
    private Vector3 recoilOffset;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        vcam = GetComponent<CinemachineVirtualCameraBase>();
        noise = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        if (recoilTarget == null)
            recoilTarget = transform;

        recoilOriginalPos = recoilTarget.localPosition;
    }

    private void Update()
    {
        UpdateShake();
        UpdateRecoil();
        UpdateTilt();
    }

    // =======================
    //      EXTREME SHAKE
    // =======================
    public void ShakeExtreme()
    {
        Shake(extremeIntensity, extremeDuration);
    }

    public void Shake(float intensity, float duration)
    {
        if (noise == null) return;

        targetAmplitude = intensity;
        shakeDuration = duration;
        shakeTimer = duration;

        noise.AmplitudeGain = intensity;
        noise.FrequencyGain = intensity * 0.7f;   // speed goyang = intensity * 0.7
    }

    void UpdateShake()
    {
        if (shakeTimer <= 0f) 
        {
            noise.AmplitudeGain = baseAmplitude;
            noise.FrequencyGain = baseFrequency;
            return;
        }

        shakeTimer -= Time.deltaTime;
        float t = 1f - (shakeTimer / shakeDuration);

        noise.AmplitudeGain = Mathf.Lerp(targetAmplitude, baseAmplitude, t);
        noise.FrequencyGain = Mathf.Lerp(targetAmplitude * 0.7f, baseFrequency, t);
    }

    // =======================
    //     RECOIL EXTREME
    // =======================
    public void RecoilKickExtreme()
    {
        Vector3 kick = new Vector3(
            -recoilDistance, 
            Random.Range(-recoilRandom, recoilRandom), 
            0f
        );

        recoilOffset += kick;
        currentTilt += Random.Range(-tiltAmount, tiltAmount);
    }

    void UpdateRecoil()
    {
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
        recoilTarget.localPosition = recoilOriginalPos + recoilOffset;
    }

    // =======================
    //     TILT EXTREME
    // =======================
    void UpdateTilt()
    {
        currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltSpeed);
        recoilTarget.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}
