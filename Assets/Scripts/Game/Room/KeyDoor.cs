using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private ParticleSystem pickupParticle;
    private bool hasTriggered;

    public void SetDoorController(DoorController controller)
    {
        doorController = controller;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            doorController.SetDoor(true);
            Instantiate(pickupParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
