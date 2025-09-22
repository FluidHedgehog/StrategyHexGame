using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathVFX : MonoBehaviour
{
    [SerializeField] Tilemap hoverMap;
    [SerializeField] Tilemap stepMap;
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
            stepMap.SetTile(step, pathTile);
        }
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
