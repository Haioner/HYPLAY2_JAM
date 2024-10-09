using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [SerializeField] private ParticleSystem firstOpenParticle;
    [SerializeField] private bool isOpened = true;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private bool isFirstOpened;
    private bool isForever;

    private void Start()
    {
        UpdateDoor();
    }

    public void SetDoor(bool state)
    {
        if (!isForever)
            isOpened = state;

        UpdateDoor();
        IsFirstTime();
    }

    public void SetDoorOpenForever()
    {
        isForever = true;
        isOpened = true;

        UpdateDoor();
        IsFirstTime();
    }

    private void IsFirstTime()
    {
        if (!isFirstOpened && isOpened)
        {
            isFirstOpened = true;
            Instantiate(firstOpenParticle, transform);
        }
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
