using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float moveDelay = 0.1f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Wall Collider")]
    [SerializeField] private float xSize = 0.8f;
    [SerializeField] private float ySize = 0.8f;

    [Header("Flip")]
    [SerializeField] private Transform flipHolder;

    private Vector2 currentDirection = Vector2.zero;
    private Vector2 inputDirection = Vector2.zero;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float moveCooldown = 0f;
    private Vector2 startedInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    private void FixedUpdate()
    {
        if (moveCooldown > 0)
        {
            moveCooldown -= Time.fixedDeltaTime;
        }
        else
        {
            Move();
            FlipSprite();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();

        if (context.started)
        {
            startedInput = rawInput;
        }

        if(startedInput.x != 0)
        {
            if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
                inputDirection = new Vector2(Mathf.Sign(rawInput.x), 0); // Esquerda/Direita
            else
                inputDirection = new Vector2(0, Mathf.Sign(rawInput.y)); // Cima/Baixo

            if (rawInput.y != 0 && rawInput.x == 0)
                startedInput = rawInput;
        }

        if(startedInput.y != 0)
        {
            if (Mathf.Abs(rawInput.x) < Mathf.Abs(rawInput.y))
                inputDirection = new Vector2(0, Mathf.Sign(rawInput.y)); // Cima/Baixo
            else
                inputDirection = new Vector2(Mathf.Sign(rawInput.x), 0); // Esquerda/Direita

            if (rawInput.x != 0 && rawInput.y == 0)
                startedInput = rawInput;
        }


        if (context.canceled)
        {
            inputDirection = Vector2.zero;
        }
    }

    private void Move()
    {
        if (isMoving)
        {
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, playerController.Speed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                isMoving = false;

                if (inputDirection != Vector2.zero)
                {
                    TryMoveToNextCell();
                }
            }
        }

        else if (inputDirection != Vector2.zero && moveCooldown <= 0)
        {
            TryMoveToNextCell();
        }
    }

    private void TryMoveToNextCell()
    {
        Vector2 nextPosition = rb.position + inputDirection;

        if (!CheckCollisionInDirection(inputDirection))
        {
            targetPosition = nextPosition;
            isMoving = true;
            currentDirection = inputDirection;

            moveCooldown = moveDelay;
        }
    }

    private bool CheckCollisionInDirection(Vector2 direction)
    {
        Vector3 boxPosition = (Vector3)rb.position + new Vector3(direction.x, direction.y, 0);
        Collider2D hit = Physics2D.OverlapBox(boxPosition, new Vector2(xSize, ySize), 0, wallLayer);
        return hit != null;
    }

    private void FlipSprite()
    {
        if(inputDirection.x != 0)
        {
            flipHolder.localScale = new Vector3(Mathf.Sign(inputDirection.x), 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (inputDirection != Vector2.zero)
        {
            Vector3 boxPosition = transform.position + new Vector3(inputDirection.x, inputDirection.y, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxPosition, new Vector3(xSize, ySize, 0));
        }
    }
}
