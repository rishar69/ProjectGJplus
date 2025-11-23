using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputAction playerInput;

    private Vector2 movement;
    private Rigidbody2D rb;
    public bool isShielding = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {


        if (isShielding)
        {
            movement = Vector2.zero; // prevent input while shielding
            return;
        }

        movement = playerInput.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;

        // Ensure player stops completely when shielding
        if (isShielding)
            rb.linearVelocity = Vector2.zero;
    }
}
