using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfinderVFX : MonoBehaviour
{
    [SerializeField] Tilemap hoverMap;
    [SerializeField] TileBase availableTile;
    [SerializeField] TileBase pathTile;


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
            hoverMap.SetTile(step, pathTile);
        }
    }

    public void ClearHighlights()
    {
        hoverMap.ClearAllTiles();
    }

    // To set Highlight Path before the movement will begin, I should call HighlightPath method from PathfinderController in the MoveUnit method.
    
}
