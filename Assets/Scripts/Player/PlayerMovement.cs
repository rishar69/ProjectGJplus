using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputAction playerInput;
    private Vector2 movement;
    private Rigidbody2D rb;
    

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
        movement = playerInput.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2 (movement.x, movement.y)*moveSpeed;
    }
}
