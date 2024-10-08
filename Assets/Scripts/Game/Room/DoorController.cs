using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [SerializeField] private ParticleSystem firstOpenParticle;
    [SerializeField] private bool isOpened = true;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    private bool isFirstOpened;

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
