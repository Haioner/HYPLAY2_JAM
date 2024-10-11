using UnityEngine;

public class IceGroundTile : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out MovementController playerMovement))
        {
            if (!playerMovement.GetIsMoving())
                playerMovement.ForceMove();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out MovementController playerMovement))
        {
            playerMovement.StopForceMove();
        }
    }
}
