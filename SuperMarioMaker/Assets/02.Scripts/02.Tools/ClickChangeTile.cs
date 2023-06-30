using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static ToolManager;
using static UnityEditor.PlayerSettings;

public class ClickChangeTile : MonoBehaviour, ICommand
{
    public Tilemap tilemap;
    public LayerMask tilemapLayer;  // 타일맵 레이어에 대한 레이어 마스크

    public Stack<GameObjectTileData> commandStack = new Stack<GameObjectTileData>();
    public Stack<GameObjectTileData> undoStack = new Stack<GameObjectTileData>();
    //private string pickedName;
    //public string PickedName
    //{
    //    get { return pickedName; }
    //    set { pickedName = value; }
    //}

    void Update()
    {
        var mode = ToolManager.Instance.ToolMode;
        if (mode == ToolModeType.Tool)
            if (Input.GetMouseButtonUp(0))
            {
                // UI 위에 마우스가 있는지 검사
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI 위에 마우스가 있다면 레이캐스팅을 무시하고 종료

                // 마우스의 스크린 좌표를 월드 좌표로 변환
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                // 타일맵에서의 클릭 위치 계산
                Vector3Int cellPos = tilemap.WorldToCell(worldPos);

                // 맵 위에 찍는 행동
                Execute(cellPos);

            }
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    // Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        //    Vector3 cellPos = tilemap.WorldToCell(worldPos);
        //    //cellPos.x += 0.5f;
        //    //cellPos.y += 0.5f;
        //    var nowName = ToolManager.Instance.iconManager.GetNowName;
        //    ResourceManager.instance.SpawnPrefabByName(nowName, cellPos);
        //    Logger.Debug(cellPos);
        //}
        //Logger.Debug(pickedName);
    }

    public void TestFunc(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Logger.Debug("우");
        }



    }
    public void AwTestFunc(InputAction.CallbackContext context)
    {

        var dir = context.ReadValue<float>();

        //if (context.started)
        //{
        //    if (dir > 0)
        //    {
        //        Logger.Debug("우");
        //    }
        //    else
        //    {
        //        Logger.Debug("좌");

        //    }
        //}
        //if (context.performed)
        {
            if (context.ReadValue<float>() > 0)
            {
                Logger.Debug("우");
            }
            else if (context.ReadValue<float>() < 0)
            {
                Logger.Debug("좌");
            }
        }
    }
    void ChangeTileUnderMouse()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        //tilemap.SetTile(cellPos, customTile);
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
    private void Start()
    {
        // LayerMask를 "Tilemap" 레이어로 설정
        //tilemapLayer = LayerMask.GetMask("Tilemap");
    }


    public void Execute(Vector3Int pos)
    {
        var nowName = ToolManager.Instance.iconManager.GetNowName;
        var gameObj = ResourceManager.instance.GetSpawnPrefabByName(nowName, pos);

        
        GameObjectTileData gameObjectTileData = new GameObjectTileData();
        gameObjectTileData.gameObject = gameObj;
        gameObjectTileData.tileData.X = pos.x;
        gameObjectTileData.tileData.Y = pos.y;
        gameObjectTileData.tileData.TileName = nowName;
        gameObjectTileData.tileData.TileType = gameObj.GetComponent<PrefapInfo>().TypeName;

        commandStack.Push(gameObjectTileData);
    }

    public void Undo()
    {
        undoStack.Push(commandStack.Pop());
    }

    public void Redo()
    {
        commandStack.Push(undoStack.Pop());
    }

}
