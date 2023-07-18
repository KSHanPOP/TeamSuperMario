using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static ToolManager;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public enum PickMode
{
    None,
    Tile,
    Player,
    Pick,
    Eraser,
}
public class ClickChangeTile : MonoBehaviour, ICommand
{
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private Button playerButton;

    public Tilemap tilemap;

    public Stack<List<GameObjectTileData>> commandStack = new Stack<List<GameObjectTileData>>();
    public Stack<List<GameObjectTileData>> undoStack = new Stack<List<GameObjectTileData>>();

    [SerializeField] private Texture2D cursorTextureBefore; // 바꿀 커서 이미지를 에디터에서 할당
    [SerializeField] private Texture2D cursorTextureAfter; // 바꿀 커서 이미지를 에디터에서 할당
    [SerializeField] private Texture2D eraserTextureBefore; // 바꿀 커서 이미지를 에디터에서 할당
    [SerializeField] private Texture2D eraserTextureAfter; // 바꿀 커서 이미지를 에디터에서 할당
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero; // 커서 이미지에서 클릭 지점의 위치. (0,0)이면 왼쪽 위 꼭짓점

    private PickMode pickerMode = PickMode.None;
    public PickMode PickerMode
    {
        get { return pickerMode; }
        set
        {
            var before = pickerMode;

            if (before == PickMode.Pick && value != PickMode.None) return;
            if (before == PickMode.Player && value != PickMode.None) return;

            if (pickerMode == value) return;

            if (pickerMode == PickMode.Eraser)
                Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);

            pickerMode = value;

            switch (pickerMode)
            {
                case PickMode.None:
                    ToolManager.Instance.iconManager.SetOUtLineOff();
                    break;
                case PickMode.Tile:
                    break;
                case PickMode.Player:
                    break;
                case PickMode.Pick:
                    break;
                case PickMode.Eraser:
                    Cursor.SetCursor(eraserTextureBefore, hotSpot, cursorMode);
                    break;
            }

            Logger.Debug(value);
        }
    }

    private bool isPlayerMove = false;

    private void SetIsPlayerMove()
    {
        PickerMode = PickMode.Player;
        isPlayerMove = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveData();
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

    public void MouseLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            return;
        var mode = ToolManager.Instance.ToolMode;

        if (mode == ToolModeType.Tool)
            switch (PickerMode)
            {
                case PickMode.None:
                    if (context.started)
                    {
                        Cursor.SetCursor(cursorTextureAfter, hotSpot, cursorMode);
                    }
                    else if (context.canceled)
                    {
                        Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);
                    }
                    break;

                case PickMode.Tile:
                    if (context.started)
                    {
                        Cursor.SetCursor(cursorTextureAfter, hotSpot, cursorMode);
                    }
                    else if (context.canceled)
                    {
                        // UI 위에 마우스가 있는지 검사
                        if (EventSystem.current.IsPointerOverGameObject())
                            return; // UI 위에 마우스가 있다면 레이캐스팅을 무시하고 종료

                        // 맵 위에 찍는 행동
                        Execute();
                        CheckUndoButtonStatus();
                        Logger.Debug("왜 2번 찍힘? ");
                        Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);
                    }
                    break;

                case PickMode.Player:
                    if (context.started)
                    {
                        Cursor.SetCursor(cursorTextureAfter, hotSpot, cursorMode);
                    }
                    else if (context.canceled)
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
                            Collider2D hitCollider = Physics2D.OverlapBox(worldPos, new Vector2(1, 1), 0f, layerMask);
                            List<GameObjectTileData> gameObjectTileDatas = new List<GameObjectTileData>();

                            if (hitCollider == null)
                            {
                                isPlayerMove = false;
                            }
                        }

                        Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);
                        PickerMode = PickMode.None;
                    }
                    break;

                case PickMode.Pick:
                    if (context.started)
                    {
                        Cursor.SetCursor(cursorTextureAfter, hotSpot, cursorMode);
                    }
                    else if (context.canceled)
                    {
                        Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);
                    }
                    break;

                case PickMode.Eraser:
                    if (context.started)
                    {
                        Cursor.SetCursor(eraserTextureAfter, hotSpot, cursorMode);
                    }
                    else if (context.canceled)
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
                        Cursor.SetCursor(eraserTextureBefore, hotSpot, cursorMode);
                    }
                    break;
            }


    }
    public void MouseRightClick(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
            return;

        if (PickerMode == PickMode.None)
        {
            PickerMode = PickMode.Eraser;
        }
        else if (PickerMode == PickMode.Eraser || PickerMode == PickMode.Tile || PickerMode == PickMode.Pick)
        {
            PickerMode = PickMode.None;
        }

        if (context.canceled)
        {
            Logger.Debug("눌림");
        }
    }
    private void Start()
    {
        redoButton.onClick.AddListener(Redo);
        undoButton.onClick.AddListener(Undo);
        playerButton.onClick.AddListener(SetIsPlayerMove);
        CheckUndoButtonStatus();
        Cursor.SetCursor(cursorTextureBefore, hotSpot, cursorMode);
        // mouseClickAction = actionMap.FindAction("MouseClick");
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

    public void LoadData(Stack<List<GameObjectTileData>> data)
    {
        commandStack.Clear();
        undoStack.Clear();

        while (data.Count != 0)
        {
            commandStack.Push(data.Pop());
        }
    }

    public List<TileData> SaveData()
    {
        Stack<List<GameObjectTileData>> tempStack = new Stack<List<GameObjectTileData>>();
        List<TileData> data = new List<TileData>();

        // Copy the original stack to the temporary stack and the data stack
        while (commandStack.Count > 0)
        {
            List<GameObjectTileData> command;// = new List<GameObjectTileData>();

            command = commandStack.Pop();

            tempStack.Push(command);

            foreach (var item in command)
            {
                if (item.gameObject.activeSelf)
                {
                    data.Add(item.tileData);
                }
            }
        }

        // Restore the original stack
        while (tempStack.Count > 0)
        {
            commandStack.Push(tempStack.Pop());
        }

        // Return the data stack
        return data;
    }
}
