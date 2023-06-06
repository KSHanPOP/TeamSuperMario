using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrawing : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(worldPos);
            tilemap.SetTile(gridPos, tile);
        }
    }
}