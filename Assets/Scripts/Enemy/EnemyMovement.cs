using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}
