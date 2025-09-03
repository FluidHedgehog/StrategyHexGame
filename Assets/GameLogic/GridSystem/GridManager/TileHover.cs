using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHover : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase hoverTile;

    [SerializeField] InputManager inputManager;
    
    private Vector3Int previousPosition;

    private void Update()
    {
        inputManager.mousePos = Camera.main.ScreenToWorldPoint(inputManager.mousePos);
        Vector3Int gridPosition = tilemap.WorldToCell(inputManager.mousePos);

        if (gridPosition != previousPosition)
        {
            tilemap.SetTile(previousPosition, null);
            tilemap.SetTile(gridPosition, hoverTile);
            previousPosition = gridPosition;
            Debug.Log($"Hovering over gridPos {gridPosition}");
        }
    }

    private void CreateTile(TileBase tile)
    {
        hoverTile = tile;
    }
}
