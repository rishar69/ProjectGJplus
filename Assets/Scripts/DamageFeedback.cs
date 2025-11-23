using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("Blink Settings")]
    [Tooltip("Kalau dikosongkan, akan otomatis ambil semua SpriteRenderer di child (untuk rig 2D).")]
    public SpriteRenderer[] spriteRenderers;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.white;

    private Color[] originalColors;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    private Rigidbody2D rb;

    public float stunDuration = 0.15f;
    public bool stunned = false;

    private void Awake()
    {
        // Ambil semua SpriteRenderer di child kalau belum diisi manual
        if (spriteRenderers == null || spriteRenderers.Length == 0)
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        // Simpan warna asli per sprite (karena rig 2D biasanya banyak sprite)
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }

        // Connect ke event Health
        var health = GetComponent<Health>();
        if (health != null)
            health.OnHealthChanged.AddListener(OnDamageTaken);
        else
            Debug.LogWarning("[DamageFeedback] Health component tidak ditemukan di " + gameObject.name);
    }

    private void OnDamageTaken(float currentHealth)
    {
        Flash();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null && move.movementType == EnemyMovement.MovementType.FixedDirection)
        {
            // Musuh yang gerak 1 arah tidak di-knockback
            return;
        }

        StartCoroutine(KnockbackRoutine(direction));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        if (rb == null)
            yield break;

        stunned = true;

        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = direction.normalized * knockbackForce;

        yield return new WaitForSeconds(stunDuration);

        stunned = false;
    }

    private void Flash()
    {

        StopCoroutine(nameof(FlashRoutine));
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Set semua sprite ke flashColor
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // Balik ke warna asli
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = originalColors[i];
        }
    }
}
