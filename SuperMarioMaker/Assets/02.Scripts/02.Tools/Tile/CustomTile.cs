using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CustomTiles/CustomTile", fileName = "CustomTile")]
public class CustomTile : Tile
{
    [SerializeField]
    private TileSetType tileType;
    public TileSetType TileType
    {
        get;
    }

    [SerializeField]
    private string tileName;
    public string TileName
    {
        get { return tileName; }
    }
}

public enum TileSetType
{
    Grid,
    StaticTile,
    DynamicTile,
    Gimmick,
    Monster,
    Item,
    Player,
}