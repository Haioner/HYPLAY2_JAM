using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private ParticleSystem pickupParticle;
    [SerializeField] private AudioClip keyClip;
    private bool hasTriggered;

    public void SetDoorController(DoorController controller)
    {
        doorController = controller;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            SoundManager.PlayAudioClip(keyClip);
            hasTriggered = true;
            doorController.SetDoorOpenForever();
            Instantiate(pickupParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
