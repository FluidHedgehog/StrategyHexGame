using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathVFX : MonoBehaviour
{
    [SerializeField] Tilemap hoverMap;
    [SerializeField] Tilemap stepMap;
    [SerializeField] Tilemap unitHoverMap;

    [SerializeField] TileBase availableTile;
    [SerializeField] TileBase pathTile;
    [SerializeField] TileBase attackTile;
    [SerializeField] TileBase friendlyTile;
    [SerializeField] TileBase ownTile;


    public void HighlightAvailableTiles(List<Vector3Int> reachable)
    {
        foreach (var tile in reachable)
        {
            hoverMap.SetTile(tile, availableTile);
        }
    }

    public void HighlightPath(List<Vector3Int> path)
    {
        foreach (var step in path)
        {
            stepMap.SetTile(step, pathTile);
        }
    }

    public void HighlightEnemy(Vector3Int enemy)
    {
        ClearUnit();
        unitHoverMap.SetTile(enemy, attackTile);
    }

    public void HighlightFriend(Vector3Int friend)
    {
        ClearUnit();
        unitHoverMap.SetTile(friend, friendlyTile);
    }

    public void HighlightOwn(Vector3Int own)
    {
        ClearUnit();
        unitHoverMap.SetTile(own, ownTile);
    }

    public void ClearUnit()
    {
        unitHoverMap.ClearAllTiles();
    }
    
    // To hover path from the unit to the mouse position, I should call HighlightPath method from PathfinderController in the MoveUnit method.

    public void ClearHighlights()
    {
        hoverMap.ClearAllTiles();
    }

    // To set Highlight Path before the movement will begin, I should call HighlightPath method from PathfinderController in the MoveUnit method.

    public void ClearPath()
    {
        stepMap.ClearAllTiles();
    }

}
