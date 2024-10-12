using UnityEngine;

public class GodRayController : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxPositionX;
    [SerializeField] private Vector2 minMaxPositionY;
    [SerializeField] private SpriteRenderer[] godRaysRenderers;
    [SerializeField] private Sprite[] godRaySprites;

    private void Start()
    {
        foreach (var ray in godRaysRenderers)
        {
            //Active
            ray.enabled = Random.value > 0.5f ? true : false;

            //Sprite
            int randSprite = Random.Range(0, godRaySprites.Length);
            ray.sprite = godRaySprites[randSprite];

            //Position
            Vector2 pos = ray.transform.localPosition;
            pos.x = Random.Range(minMaxPositionX.x, minMaxPositionX.y);
            pos.y = Random.Range(minMaxPositionY.x, minMaxPositionY.y);
            ray.transform.localPosition = pos;
        }
    }
}
