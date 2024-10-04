using UnityEngine.InputSystem;
using UnityEngine;

public class Pacman_Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float coyoteTime = 0.5f;

    [Header("Wall Collider")]
    [SerializeField] private float xSize = 1f;
    [SerializeField] private float ySize = 1f;

    private Vector2 currentDirection = Vector2.right;
    private Vector2 inputDirection;
    private Vector2 savedInput;
    private float coyoteTimer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleCoyoteTime();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
            inputDirection = new Vector2(Mathf.Sign(rawInput.x), 0); // Left and right
        else if (Mathf.Abs(rawInput.y) > Mathf.Abs(rawInput.x))
            inputDirection = new Vector2(0, Mathf.Sign(rawInput.y)); // Up and down

        if (inputDirection != Vector2.zero)
        {
            savedInput = inputDirection;
            coyoteTimer = coyoteTime;
        }
    }

    private void Move()
    {
        rb.velocity = currentDirection * playerController.Speed;
    }

    private void HandleCoyoteTime()
    {
        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.fixedDeltaTime;

            if (!CheckCollisionInDirection(savedInput))
            {
                currentDirection = savedInput;
                coyoteTimer = 0;
            }
        }
    }

    private bool CheckCollisionInDirection(Vector2 direction)
    {
        Vector3 boxPosition = transform.position + new Vector3(direction.x, direction.y, 0);
        Collider2D hit = Physics2D.OverlapBox(boxPosition, new Vector2(xSize, ySize), 0);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        if (savedInput != Vector2.zero)
        {
            Vector3 boxPosition = transform.position + new Vector3(savedInput.x, savedInput.y, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxPosition, new Vector3(xSize, ySize, 0));
        }
    }
}
