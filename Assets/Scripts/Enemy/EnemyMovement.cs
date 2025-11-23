using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType
    {
        Chase,
        FixedDirection
    }

    public enum Direction4
    {
        Up,
        Down,
        Left,
        Right
    }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.Chase;

    [Header("Chase Settings")]
    public Transform target;

    [Header("Fixed Direction Settings")]
    public Direction4 moveDirection = Direction4.Up;
    public float destroyDistance = 20f;   

    public float speed = 5f;

    private Rigidbody2D rb;
    private DamageFeedback feedback;

    private Vector3 spawnPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        feedback = GetComponent<DamageFeedback>();
        spawnPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Chase)
        {
            if (feedback != null && feedback.stunned)
                return;

            ChaseMovement();
        }
        else if (movementType == MovementType.FixedDirection)
        {
            MoveInChosenDirection();
            CheckAutoDestroy();
        }
    }

    private void ChaseMovement()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void MoveInChosenDirection()
    {
        Vector2 dir = Vector2.zero;

        switch (moveDirection)
        {
            case Direction4.Up: dir = Vector2.up; break;
            case Direction4.Down: dir = Vector2.down; break;
            case Direction4.Left: dir = Vector2.left; break;
            case Direction4.Right: dir = Vector2.right; break;
        }

        rb.linearVelocity = dir * speed;
    }

    private void CheckAutoDestroy()
    {
        //float dist = Vector3.Distance(transform.position, spawnPosition);

        //if (dist >= destroyDistance)
        //{
        //    Destroy(gameObject);
        //}
    }
}
