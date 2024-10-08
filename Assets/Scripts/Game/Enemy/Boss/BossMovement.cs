using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 targetPosition;
    public bool IsMoving { get; private set; }

    public void MoveBoss(Vector3 targetPosition)
    {
        anim.Play("Leave");
        this.targetPosition = targetPosition;
    }

    public void StartMove_EVENT()
    {
        IsMoving = true;
        StopAllCoroutines();
        StartCoroutine(SmoothMove());
    }

    private System.Collections.IEnumerator SmoothMove()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        anim.Play("Enter");
        IsMoving = false;
    }
}
