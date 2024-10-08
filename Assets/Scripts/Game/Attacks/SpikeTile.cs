using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeTile : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase spikeHoleTile;
    [SerializeField] private TileBase spikeActivatedTile;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private float spikeActiveDuration = 2f;
    [SerializeField] private int damageAmount = 1;
    private HashSet<Vector3Int> activatedTiles = new HashSet<Vector3Int>();

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamage damageable))
        {
            Vector3Int tilePosition = GetTilePosition(collision);

            if (tilemap.GetTile(tilePosition) == spikeHoleTile && !activatedTiles.Contains(tilePosition))
            {
                activatedTiles.Add(tilePosition);
                StartCoroutine(ActivateSpike(tilePosition, collision.gameObject));
            }
            
            if (tilemap.GetTile(tilePosition) == spikeActivatedTile)
                DealDamage(collision.gameObject);
        }
    }

    private Vector3Int GetTilePosition(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        }
        return tilemap.WorldToCell(hitPosition);
    }

    private IEnumerator ActivateSpike(Vector3Int tilePosition, GameObject character)
    {
        yield return new WaitForSeconds(damageDelay);
        tilemap.SetTile(tilePosition, spikeActivatedTile);

        Vector3Int playerTilePosition = tilemap.WorldToCell(character.transform.position);
        if (playerTilePosition == tilePosition && character != null)
        {
            DealDamage(character);
        }

        yield return new WaitForSeconds(spikeActiveDuration);
        tilemap.SetTile(tilePosition, spikeHoleTile);
        activatedTiles.Remove(tilePosition);
    }

    private void DealDamage(GameObject character)
    {
        IDamage damageable = character.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.Damage(damageAmount);
        }
    }
}
