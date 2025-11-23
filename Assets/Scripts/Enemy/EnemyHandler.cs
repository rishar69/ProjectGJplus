using UnityEngine;
using System.Collections;

public class EnemyHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer[] renderers;

    [Header("Dissolve Settings")]
    [SerializeField] private float dissolveSpeed = 1.5f;

    // Sesuaikan dengan Reference di Shader Graph
    private const string DissolveProperty = "_Fade";

    private bool isDead = false;
    private bool isDissolving = false;   // <- cegah coroutine ganda
    private Material[] dissolveMaterials;

    private void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isDead = false;
        isDissolving = false;
        dissolveMaterials = null;

        if (health != null)
            health.OnDeath.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDeath.RemoveListener(HandleDeath);

        CleanUpMaterials();
    }

    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        DisableMovement();

        if (animator != null)
            animator.SetTrigger("Death");

        // PILIH SALAH SATU:
        // 1) langsung dissolve (baris di bawah aktif)
        // 2) pakai AnimationEvent -> COMMENT 2 baris di bawah dan panggil StartDissolveFromAnim dari anim
        StartDissolve();
    }

    private void DisableMovement()
    {
        if (TryGetComponent<EnemyMovement>(out var move))
            move.enabled = false;

        if (TryGetComponent<Collider2D>(out var col))
            col.enabled = false;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    // ---- DIPAKAI KODE & ANIMATION EVENT ----
    public void StartDissolveFromAnim()
    {
        // supaya aman kalau dipanggil dari anim
        StartDissolve();
    }

    private void StartDissolve()
    {
        // jangan mulai lagi kalau sudah jalan
        if (isDissolving) return;
        isDissolving = true;

        InitializeDissolveMaterials();
        if (dissolveMaterials == null || dissolveMaterials.Length == 0)
        {
            Debug.LogWarning($"{name}: No dissolve materials, destroying immediately.");
            Destroy(gameObject);
            return;
        }

        StartCoroutine(DissolveRoutine());
    }

   private void InitializeDissolveMaterials()
{
    if (renderers == null || renderers.Length == 0)
    {
        Debug.LogWarning($"{name}: No SpriteRenderers found.");
        return;
    }

    dissolveMaterials = new Material[renderers.Length];

    for (int i = 0; i < renderers.Length; i++)
    {
        var sr = renderers[i];
        if (sr == null)
        {
            Debug.LogWarning($"{name}: Renderer {i} is null");
            continue;
        }

        var mat = sr.material;
        dissolveMaterials[i] = mat;

        if (mat.HasProperty(DissolveProperty))
        {
            mat.SetFloat(DissolveProperty, 50f);
            Debug.Log($"{name}: INIT Material[{i}] = {sr.name} → SetFade = 50");
        }
        else
        {
            Debug.LogWarning($"{name}: Material[{i}] ({sr.name}) HAS NO PROPERTY {DissolveProperty}");
        }
    }

    Debug.Log($"{name}: Total dissolve materials initialized = {dissolveMaterials.Length}");
}    private IEnumerator DissolveRoutine()
{
    if (dissolveMaterials == null)
    {
        Debug.LogError($"{name}: dissolveMaterials is NULL!");
        Destroy(gameObject);
        yield break;
    }

    yield return new WaitForSeconds(0.25f);
    Debug.Log($"{name}: >>> START DISSOLVE <<<");

    float t = 0f;
    float fadeStart = 50f;
    float fadeEnd = 0f;

    bool printed25 = false;
    bool printed50 = false;
    bool printed75 = false;

    while (t < 1f)
    {
        t += Time.deltaTime * dissolveSpeed;
        float fadeValue = Mathf.Lerp(fadeStart, fadeEnd, t);

        // apply fade
        for (int i = 0; i < dissolveMaterials.Length; i++)
        {
            var mat = dissolveMaterials[i];
            if (mat != null && mat.HasProperty(DissolveProperty))
                mat.SetFloat(DissolveProperty, fadeValue);
        }

        // print debug at checkpoints (biar gak flooding)
        if (!printed25 && fadeValue <= 37.5f)
        {
            printed25 = true;
            Debug.Log($"{name}: Fade ~75% → {fadeValue}");
        }
        if (!printed50 && fadeValue <= 25f)
        {
            printed50 = true;
            Debug.Log($"{name}: Fade ~50% → {fadeValue}");
        }
        if (!printed75 && fadeValue <= 12.5f)
        {
            printed75 = true;
            Debug.Log($"{name}: Fade ~25% → {fadeValue}");
        }

        yield return null;
    }

    Debug.Log($"{name}: >>> END DISSOLVE, Fade = 0 <<<");

    CleanUpMaterials();
    Destroy(gameObject);
}

    private void CleanUpMaterials()
    {
        if (dissolveMaterials == null) return;

        for (int i = 0; i < dissolveMaterials.Length; i++)
        {
            var mat = dissolveMaterials[i];
            if (mat != null)
                Destroy(mat);
        }

        dissolveMaterials = null;
    }
}
