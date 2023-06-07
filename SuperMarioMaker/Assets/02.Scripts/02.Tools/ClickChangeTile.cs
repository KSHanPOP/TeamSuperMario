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
        //if (Input.GetMouseButtonDown(0)) // 0 : left click, 1 : right click
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("Clicked on the UI");
        //    }
        //    else
        //    {
        //        ChangeTileUnderMouse();
        //        Debug.Log("Clicked on the game object");
        //    }
        //}
    }

    public void TestFunc(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Logger.Debug("©Л");
        }



    }
    public void AwTestFunc(InputAction.CallbackContext context)
    {

        var dir = context.ReadValue<float>();

        //if (context.started)
        //{
        //    if (dir > 0)
        //    {
        //        Logger.Debug("©Л");
        //    }
        //    else
        //    {
        //        Logger.Debug("аб");

        //    }
        //}
        //if (context.performed)
        {
            if (context.ReadValue<float>() > 0)
            {
                Logger.Debug("©Л");
            }
            else if(context.ReadValue<float>() < 0)
            {
                Logger.Debug("аб");

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
