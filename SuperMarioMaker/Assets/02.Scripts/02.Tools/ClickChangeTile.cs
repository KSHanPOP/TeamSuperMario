using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class ClickChangeTile : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile newTile;
    public CustomTile customTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 : left click, 1 : right click
        {
            ChangeTileUnderMouse();
        }
    }

    void ChangeTileUnderMouse()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        tilemap.SetTile(cellPos, customTile);
        Logger.Debug(cellPos);
        Logger.Debug(GetCustomTileName(cellPos));
    }

    public string GetCustomTileName(Vector3Int pos)
    {
        TileBase tileBase = tilemap.GetTile(pos);
        CustomTile customTile = tileBase as CustomTile;
        if (customTile != null)
        {
            return customTile.tileName;
        }

        return null;
    }
}
