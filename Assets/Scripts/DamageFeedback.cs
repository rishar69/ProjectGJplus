using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("Blink Settings")]
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.white;

    private Color originalColor;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    private Rigidbody2D rb;

    public float stunDuration = 0.15f;
    public bool stunned = false;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        originalColor = spriteRenderer.color;

        // Connect to Health event
        GetComponent<Health>().OnHealthChanged.AddListener(OnDamageTaken);
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
            return; 
        }

        StartCoroutine(KnockbackRoutine(direction));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        stunned = true;

        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = direction.normalized * knockbackForce;

        yield return new WaitForSeconds(stunDuration);

        stunned = false;
    }

    private void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
