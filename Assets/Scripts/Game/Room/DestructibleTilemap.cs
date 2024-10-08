using UnityEngine.Tilemaps;
using UnityEngine;

public class DestructibleTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private string explosionLayer = "Attack";
    [SerializeField] private ParticleSystem destructiveParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(explosionLayer))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
            if (tilemap.HasTile(tilePosition))
            {
                DestroyTile(tilePosition);
            }
        }
    }
    private void DestroyTile(Vector3Int tilePosition)
    {
        Instantiate(destructiveParticle, tilePosition, Quaternion.identity);
        tilemap.SetTile(tilePosition, null);
        tilemap.RefreshTile(tilePosition);
    }
}
