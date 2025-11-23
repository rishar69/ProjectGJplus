using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float lifeTime = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        Health h = collision.GetComponentInParent<Health>();
        DamageFeedback feedback = collision.GetComponentInParent<DamageFeedback>();

        if (h != null)
            h.TakeDamage(damage);

        if (feedback != null)
        {
            Vector2 knockDir = (collision.transform.position - transform.position).normalized;
            feedback.ApplyKnockback(knockDir);
        }

        Destroy(gameObject);
    }

}
