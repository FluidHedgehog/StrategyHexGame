using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    [Header("Allowed")]
    [SerializeField] public bool walkable;
    [SerializeField] public bool swimable;
    [SerializeField] public bool flyable;

    [Header("Cost")]
    [SerializeField] public int walkCost;
    [SerializeField] public int swimCost;
    [SerializeField] public int flyCost;

}
