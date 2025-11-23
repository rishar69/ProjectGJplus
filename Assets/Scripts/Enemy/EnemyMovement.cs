using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType { Chase, FixedDirection }
    public enum Direction4 { Up, Down, Left, Right }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.Chase;

    [Header("Chase Settings")]
    public Transform target;

    [Header("Fixed Direction Settings")]
    public Direction4 moveDirection = Direction4.Up;
    public float destroyDistance = 20f;   

    public float speed = 5f;

    [Header("Visual / Animation")]
    public Transform visualRoot;      
    public bool facingRight = true;   
    private Animator anim;

    private Rigidbody2D rb;
    private DamageFeedback feedback;
    private Vector3 spawnPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        feedback = GetComponent<DamageFeedback>();
        
        if (visualRoot != null)
            anim = visualRoot.GetComponentInChildren<Animator>();
        else
            anim = GetComponentInChildren<Animator>();

        spawnPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Chase)
        {
            if (feedback != null && feedback.stunned)
            {
                UpdateAnimation(Vector2.zero);
                return;
            }

            Vector2 dir = ChaseMovement();
            HandleFlip(dir);
            UpdateAnimation(dir);
        }
        else if (movementType == MovementType.FixedDirection)
        {
            Vector2 dir = MoveInChosenDirection();
            HandleFlip(dir);
            UpdateAnimation(dir);
            CheckAutoDestroy();
        }
    }

    private Vector2 ChaseMovement()
    {
        if (target == null) return Vector2.zero;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        return direction;
    }

    private Vector2 MoveInChosenDirection()
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
        return dir;
    }

    private void CheckAutoDestroy()
    {
        //float dist = Vector3.Distance(transform.position, spawnPosition);

        //if (dist >= destroyDistance)
        //{
        //    Destroy(gameObject);
        //}
    }


    private void UpdateAnimation(Vector2 dir)
    {
        if (anim == null) return;

        float speedValue = rb.linearVelocity.magnitude;

        anim.SetFloat("Speed", speedValue);
        anim.SetFloat("Horizontal", dir.x);
        anim.SetFloat("Vertical", dir.y);
    }

 
    void Flip()
    {
        facingRight = !facingRight;

        if (visualRoot == null) return;

        Vector3 s = visualRoot.localScale;
        s.x *= -1;             
        visualRoot.localScale = s;
    }

    private void HandleFlip(Vector2 dir)
    {
       
        if (dir.x > 0.01f && !facingRight)
        {
            Flip();
        }
        else if (dir.x < -0.01f && facingRight)
        {
            Flip();
        }

    }
}
