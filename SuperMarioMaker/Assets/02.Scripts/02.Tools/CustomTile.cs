using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CustomTiles/CustomTile", fileName = "CustomTile")]
public class CustomTile : Tile
{
    [SerializeField]
    private string tileType;
    public string TileType
    {
        get { return tileName; }
    }

    [SerializeField]
    private string tileName;
    public string TileName
    {
        get { return tileName; }
    }
}
