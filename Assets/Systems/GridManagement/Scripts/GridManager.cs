using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridManager : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;

    public Dictionary<TileBase, TileData> dataFromTerrain;
    [SerializeField] List<TileData> tileDatas;

    private void Awake()
    {
        FindAndAddAllTilesInScene();
    }

    private void FindAndAddAllTilesInScene()
    {
        dataFromTerrain = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTerrain.Add(tile, tileData);
            }
        }
    }
}
