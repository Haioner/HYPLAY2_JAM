using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections;

public class DestructibleLava : MonoBehaviour
{
    [SerializeField] private Transform roomTarget;
    [SerializeField] private RoomController roomController;
    [SerializeField] private Tilemap destructibleTilemap;
    [SerializeField] private Tilemap lavaTilemap;
    [SerializeField] private string explosionLayer = "Attack";
    [SerializeField] private ParticleSystem destructiveParticle;
    [SerializeField] private TileBase lavaTile; // Tile de lava
    [SerializeField, Range(0f, 1f)] private float lavaSpawnChance = 0.3f;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = roomTarget.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(explosionLayer))
        {
            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            Vector3Int tilePosition = destructibleTilemap.WorldToCell(hitPosition);

            if (destructibleTilemap.HasTile(tilePosition))
            {
                HandleTileDestruction(tilePosition);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamage damageable))
            damageable.Damage(1);
    }

    private void HandleTileDestruction(Vector3Int tilePosition)
    {
        DestroyTile(tilePosition);
        if (Random.value < lavaSpawnChance)
            SpawnLavaTile(tilePosition);
        GameController.instance.UpdateProceduralGrid();
    }

    private void DestroyTile(Vector3Int tilePosition)
    {
        Instantiate(destructiveParticle, destructibleTilemap.CellToWorld(tilePosition), Quaternion.identity);
        destructibleTilemap.SetTile(tilePosition, null);
        destructibleTilemap.RefreshTile(tilePosition);
    }

    private void SpawnLavaTile(Vector3Int tilePosition)
    {
        lavaTilemap.SetTile(tilePosition, lavaTile);
        lavaTilemap.RefreshTile(tilePosition);

        TilemapCollider2D lavaTilemapCollider = lavaTilemap.GetComponent<TilemapCollider2D>();
        if (lavaTilemapCollider != null)
        {
            lavaTilemapCollider.enabled = false;
            lavaTilemapCollider.enabled = true;
        }
    }
}
