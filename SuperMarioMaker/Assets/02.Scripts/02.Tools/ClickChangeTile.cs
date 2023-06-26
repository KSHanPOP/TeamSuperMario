using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class ClickChangeTile : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile newTile;
    public CustomTile customTile;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Logger.Debug(tilemap.cellBounds.min);
            Logger.Debug(tilemap.cellBounds.max);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           // Vector3Int cellPos = tilemap.WorldToCell(worldPos);
            Vector3 cellPos = tilemap.WorldToCell(worldPos);
            cellPos.x += 0.5f;
            cellPos.y += 0.5f;
            ResourceManager.instance.SpawnPrefabByName("Goomba", cellPos);
            Logger.Debug(cellPos);
        }

    }

    public void TestFunc(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Logger.Debug("��");
        }



    }
    public void AwTestFunc(InputAction.CallbackContext context)
    {

        var dir = context.ReadValue<float>();

        //if (context.started)
        //{
        //    if (dir > 0)
        //    {
        //        Logger.Debug("��");
        //    }
        //    else
        //    {
        //        Logger.Debug("��");

        //    }
        //}
        //if (context.performed)
        {
            if (context.ReadValue<float>() > 0)
            {
                Logger.Debug("��");
            }
            else if (context.ReadValue<float>() < 0)
            {
                Logger.Debug("��");
            }
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
            return customTile.TileName;
        }

        return null;
    }
}
