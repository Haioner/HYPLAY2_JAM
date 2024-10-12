using UnityEngine;

public class MenuEyeFollow : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float minY = 47f;
    [SerializeField] private float maxY = 55f;
    [SerializeField] private float followRange = 5.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        float distanceToMouseX = mousePos.x - transform.localPosition.x;
        float distanceToMouseY = mousePos.y - transform.localPosition.y;

        float normalizedDistanceX = Mathf.Clamp(distanceToMouseX / followRange, -1, 1);
        float clampedX = Mathf.Lerp(minX, maxX, (normalizedDistanceX + 1) / 2);

        float normalizedDistanceY = Mathf.Clamp(distanceToMouseY / followRange, -1, 1);
        float clampedY = Mathf.Lerp(minY, maxY, (normalizedDistanceY + 1) / 2);

        transform.localPosition = new Vector3(clampedX, clampedY, initialPosition.z);
    }
}
