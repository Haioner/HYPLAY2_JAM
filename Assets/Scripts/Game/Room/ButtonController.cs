using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private Sprite[] buttonSprites; //0 unpressed //1 pressed
    [SerializeField] private GameObject pressedVFX;

    private SpriteRenderer spriteRenderer;
    private bool isStay;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDoorController(DoorController controller)
    {
        doorController = controller;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack")) return;
        doorController.SetDoor(true);
        spriteRenderer.sprite = buttonSprites[1];
        pressedVFX.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack")) return;
        doorController.SetDoor(false);
        spriteRenderer.sprite = buttonSprites[0];
        pressedVFX.SetActive(false);
    }
}
