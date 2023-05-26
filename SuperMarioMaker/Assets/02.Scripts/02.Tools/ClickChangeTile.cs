using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickChangeTile : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile newTile;

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
        tilemap.SetTile(cellPos, newTile);
        Logger.Debug(cellPos);
    }
}
