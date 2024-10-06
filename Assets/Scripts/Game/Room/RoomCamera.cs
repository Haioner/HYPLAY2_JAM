using Unity.Cinemachine;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cinemachineCamera.enabled = false;
        }
    }
}
