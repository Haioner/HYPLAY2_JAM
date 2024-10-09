using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float boxCheckDistance = 0.6f;
    [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.8f);

    [Header("Drop")]
    public bool hasDrop;
    [SerializeField] private KeyDoor keyDrop;
    [SerializeField] private HealthController healthController;
    [SerializeField] private DoorController doorController;

    private Vector2 pushDirection = Vector2.zero;
    private bool isBeingPushed;

    public void SetDoorController(DoorController controller)
    {
        doorController = controller;
    }

    private void OnEnable()
    {
        healthController = GetComponent<HealthController>();
        healthController.OnDeath += DropKey;
    }

    private void OnDisable()
    {
        healthController.OnDeath -= DropKey;
    }

    private void DropKey(object sender, System.EventArgs e)
    {
        if (hasDrop)
        {
            KeyDoor key = Instantiate(keyDrop, transform.position, Quaternion.identity);
            key.SetDoorController(doorController);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Box"))
        {
            Vector2 playerPosition = collision.transform.position;
            Vector2 boxPosition = transform.position;
            Vector2 difference = boxPosition - playerPosition;

            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
                pushDirection = (difference.x > 0) ? Vector2.right : Vector2.left;
            else
                pushDirection = (difference.y > 0) ? Vector2.up : Vector2.down;

            TryMoveBox(playerPosition);
            isBeingPushed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Box"))
        {
            pushDirection = Vector2.zero;
            isBeingPushed = false;
        }
    }

    private void TryMoveBox(Vector2 playerPosition)
    {
        Vector2 newPosition = (Vector2)transform.position;
        bool isBlocked = Physics2D.OverlapBox((Vector2)transform.position + pushDirection * boxCheckDistance, boxSize, 0f, obstacleLayer);

        if (!isBlocked)
            newPosition += pushDirection;
        else
        {
            bool isPerpendicularBlocked = false;

            if (pushDirection.x != 0)
            {
                Vector2 perpendicularDirection1 = Vector2.up;
                Vector2 perpendicularDirection2 = Vector2.down;

                if (!Physics2D.OverlapBox((Vector2)transform.position + perpendicularDirection1 * boxCheckDistance, boxSize, 0f, obstacleLayer))
                {
                    newPosition += perpendicularDirection1; //Up
                }
                else if (!Physics2D.OverlapBox((Vector2)transform.position + perpendicularDirection2 * boxCheckDistance, boxSize, 0f, obstacleLayer))
                {
                    newPosition += perpendicularDirection2; //Down
                }
                else
                {
                    isPerpendicularBlocked = true; //Perpendicular Blocked 
                }
            }
            else if (pushDirection.y != 0) //Vertical
            {
                Vector2 perpendicularDirection1 = Vector2.right;
                Vector2 perpendicularDirection2 = Vector2.left;

                if (!Physics2D.OverlapBox((Vector2)transform.position + perpendicularDirection1 * boxCheckDistance, boxSize, 0f, obstacleLayer))
                {
                    newPosition += perpendicularDirection1; //Right
                }
                else if (!Physics2D.OverlapBox((Vector2)transform.position + perpendicularDirection2 * boxCheckDistance, boxSize, 0f, obstacleLayer))
                {
                    newPosition += perpendicularDirection2; //Left
                }
                else
                {
                    isPerpendicularBlocked = true;
                }
            }

            if (isPerpendicularBlocked)
                newPosition = (Vector2)transform.position - pushDirection; //Backwards
        }

        transform.position = newPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 boxPosition = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPosition + Vector2.up * boxCheckDistance, boxSize);    // Cima
        Gizmos.DrawWireCube(boxPosition + Vector2.down * boxCheckDistance, boxSize);  // Baixo
        Gizmos.DrawWireCube(boxPosition + Vector2.left * boxCheckDistance, boxSize);  // Esquerda
        Gizmos.DrawWireCube(boxPosition + Vector2.right * boxCheckDistance, boxSize); // Direita

        if (pushDirection != Vector2.zero)
        {
            Gizmos.color = Color.yellow;
            Vector3 startPos = new Vector3(boxPosition.x, boxPosition.y, 0f);
            Vector3 endPos = startPos + new Vector3(-pushDirection.x, -pushDirection.y, 0f) * 0.5f;
            Gizmos.DrawLine(startPos, endPos);

            Vector3 arrowHeadDirection1 = Quaternion.Euler(0, 0, 45) * (-pushDirection);
            Vector3 arrowHeadDirection2 = Quaternion.Euler(0, 0, -45) * (-pushDirection);
            Gizmos.DrawLine(endPos, endPos + arrowHeadDirection1 * 0.2f);
            Gizmos.DrawLine(endPos, endPos + arrowHeadDirection2 * 0.2f);
        }
    }
}
