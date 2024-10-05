using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float minX = -0.15f;
    [SerializeField] private float maxX = 0.15f;
    [SerializeField] private float followRange = 5.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (target != null)
        {
            float distanceToTarget = target.position.x - transform.position.x;
            float normalizedDistance = Mathf.Clamp(distanceToTarget / followRange, -1, 1);
            float clampedX = Mathf.Lerp(minX, maxX, (normalizedDistance + 1) / 2);
            transform.localPosition = new Vector3(clampedX, initialPosition.y, initialPosition.z);
        }
    }
}
