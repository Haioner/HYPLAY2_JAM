using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool isOpened = true;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private void Start()
    {
        UpdateDoor();
    }

    public void SwitchDoor()
    {
        isOpened = !isOpened;
        UpdateDoor();
    }

    public void SetDoor(bool state)
    {
        isOpened = state;
        UpdateDoor();
    }

    private void UpdateDoor()
    {
        if (isOpened)
        {
            openEvent?.Invoke();
        }
        else
        {
            closeEvent?.Invoke();
        }
    }
}
