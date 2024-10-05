using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    [SerializeField] private GameObject cinemachineCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.SetActive(false);
        }
    }
}
