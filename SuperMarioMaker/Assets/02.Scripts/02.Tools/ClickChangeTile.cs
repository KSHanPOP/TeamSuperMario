using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static ToolManager;
using static UnityEditor.PlayerSettings;

public class ClickChangeTile : MonoBehaviour, ICommand
{
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private Button playerButton;

    public Tilemap tilemap;

    public Stack<List<GameObjectTileData>> commandStack = new Stack<List<GameObjectTileData>>();
    public Stack<List<GameObjectTileData>> undoStack = new Stack<List<GameObjectTileData>>();

    private bool isPlayerMove = false;

    private void SetIsPlayerMove()
    {
        isPlayerMove = true;
    }
    void Update()
    {
        var mode = ToolManager.Instance.ToolMode;
        if (mode == ToolModeType.Tool)
        {
            if (isPlayerMove)
            {
                // UI 위에 마우스가 있는지 검사
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI 위에 마우스가 있다면 레이캐스팅을 무시하고 종료

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                ToolManager.Instance.PlayerObj.transform.position = worldPos;

                if (Input.GetMouseButtonUp(0))
                {

                    int playerLayer = LayerMask.NameToLayer("Player"); // Player 레이어의 번호를 가져옵니다.
                    int layerMask = 1 << playerLayer; // Player 레이어를 Mask로 설정합니다.
                    layerMask = ~layerMask; // Bitwise NOT 연산을 통해 Player 레이어를 제외합니다.
                  
                    // 마우스의 스크린 좌표를 월드 좌표로 변환
                    Collider2D hitCollider = Physics2D.OverlapPoint(worldPos,layerMask);
                    List<GameObjectTileData> gameObjectTileDatas = new List<GameObjectTileData>();

                    if (hitCollider == null)
                    {
                        isPlayerMove = false;
                    }
                }
                return;
            }
            if (Input.GetMouseButtonUp(0))
            {
                // UI 위에 마우스가 있는지 검사
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI 위에 마우스가 있다면 레이캐스팅을 무시하고 종료

                // 맵 위에 찍는 행동
                Execute();
                CheckUndoButtonStatus();
                Logger.Debug("왜 2번 찍힘? ");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                // UI 위에 마우스가 있는지 검사
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI 위에 마우스가 있다면 레이캐스팅을 무시하고 종료

                // 마우스의 스크린 좌표를 월드 좌표로 변환
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                // 타일맵에서의 클릭 위치 계산
                Vector3Int cellPos = tilemap.WorldToCell(worldPos);

                Delete();
                CheckUndoButtonStatus();
            }
        }
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
        redoButton.onClick.AddListener(Redo);
        undoButton.onClick.AddListener(Undo);
        playerButton.onClick.AddListener(SetIsPlayerMove);
        CheckUndoButtonStatus();
    }

    private void CheckUndoButtonStatus()
    {
        undoButton.interactable = commandStack.Count > 0;
        redoButton.interactable = undoStack.Count > 0;
    }

    public void Execute()// 실행
    {
        if (undoStack.Count != 0)
        {
            undoStack.Clear();
        }

        // 마우스의 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 타일맵에서의 클릭 위치 계산
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos);
        List<GameObjectTileData> gameObjectTileDatas = new List<GameObjectTileData>();

        if (hitCollider != null)
        {
            var obj = hitCollider.gameObject;
            if (obj.GetComponent<PrefapInfo>().TypeName == ETileType.Default)
            {
                Logger.Debug("Default");
                return;
            }
            else
            {
                hitCollider.gameObject.SetActive(false);

                // GameObjectTileData 인스턴스를 만듭니다.
                GameObjectTileData data = new GameObjectTileData();
                data.gameObject = hitCollider.gameObject;  // 게임 오브젝트 설정
                gameObjectTileDatas.Add(data);
            }
        }



        var nowName = ToolManager.Instance.iconManager.GetNowName;
        if (nowName == null)
            return;

        var gameObj = ResourceManager.instance.GetSpawnPrefabByName(nowName, cellPos);

        GameObjectTileData gameObjectTileData = new GameObjectTileData();
        gameObjectTileData.gameObject = gameObj;
        gameObjectTileData.tileData.X = cellPos.x;
        gameObjectTileData.tileData.Y = cellPos.y;
        gameObjectTileData.tileData.TileName = nowName;
        gameObjectTileData.tileData.TileType = gameObj.GetComponent<PrefapInfo>().TypeName;

        gameObjectTileDatas.Add(gameObjectTileData);

        commandStack.Push(gameObjectTileDatas);
    }

    public void Undo()//실행 취소
    {
        if (commandStack.Count > 0)
        {
            List<GameObjectTileData> gameObjList = commandStack.Pop();

            foreach (var gameObj in gameObjList)
            {
                if (gameObj.gameObject.activeSelf)
                {
                    // GameObject 비활성화
                    gameObj.gameObject.SetActive(false);
                }
                else
                {
                    gameObj.gameObject.SetActive(true);
                }
            }

            // GameObject를 undoStack에 push
            undoStack.Push(gameObjList);
        }
        else
        {
            Debug.LogError("Nothing to undo.");
        }
        CheckUndoButtonStatus();
    }

    public void Redo()//다시
    {
        if (undoStack.Count > 0)
        {
            List<GameObjectTileData> gameObjList = undoStack.Pop();

            foreach (var gameObj in gameObjList)
            {
                if (gameObj.gameObject.activeSelf)
                {
                    // GameObject 비활성화
                    gameObj.gameObject.SetActive(false);
                }
                else
                {
                    gameObj.gameObject.SetActive(true);
                }
            }

            // GameObject를 redoStack에 push
            commandStack.Push(gameObjList);
        }
        else
        {
            Debug.LogError("Nothing to redo.");
        }
        CheckUndoButtonStatus();
    }

    public void Delete()
    {
        // 마우스의 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //// 타일맵에서의 클릭 위치 계산
        //Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos);
        List<GameObjectTileData> gameObjectTileDatas = new List<GameObjectTileData>();

        if (hitCollider != null)
        {
            var obj = hitCollider.gameObject;
            if (obj.GetComponent<PrefapInfo>().TypeName == ETileType.Default)
            {
                Logger.Debug("Default");
                return;
            }
            else
            {
                hitCollider.gameObject.SetActive(false);

                // GameObjectTileData 인스턴스를 만듭니다.
                GameObjectTileData data = new GameObjectTileData();
                data.gameObject = hitCollider.gameObject;  // 게임 오브젝트 설정
                gameObjectTileDatas.Add(data);

                // GameObject를 undoStack에 push
                commandStack.Push(gameObjectTileDatas);
            }
        }
    }



}
