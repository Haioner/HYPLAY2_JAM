using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private Sprite[] buttonSprites; //0 unpressed //1 pressed
    [SerializeField] private GameObject pressedVFX;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        door.SetDoor(true);
        spriteRenderer.sprite = buttonSprites[1];
        pressedVFX.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        door.SetDoor(false);
        spriteRenderer.sprite = buttonSprites[0];
        pressedVFX.SetActive(false);
    }
}
