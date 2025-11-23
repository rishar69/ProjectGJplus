using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction playerInput;
    public float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    public bool isShielding = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
            playerInput.Enable();
        else
            Debug.LogError("[PlayerMovement] playerInput belum di-assign di Inspector!");
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.Disable();
    }

    private void Update()
    {


        if (isShielding)
        {
            movement = Vector2.zero;
            return;
        }

        if (playerInput != null)
        {
            movement = playerInput.ReadValue<Vector2>();
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isShielding)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * moveSpeed;
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }
}
