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
                // UI ���� ���콺�� �ִ��� �˻�
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI ���� ���콺�� �ִٸ� ����ĳ������ �����ϰ� ����

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                ToolManager.Instance.PlayerObj.transform.position = worldPos;

                if (Input.GetMouseButtonUp(0))
                {

                    int playerLayer = LayerMask.NameToLayer("Player"); // Player ���̾��� ��ȣ�� �����ɴϴ�.
                    int layerMask = 1 << playerLayer; // Player ���̾ Mask�� �����մϴ�.
                    layerMask = ~layerMask; // Bitwise NOT ������ ���� Player ���̾ �����մϴ�.
                  
                    // ���콺�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
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
                // UI ���� ���콺�� �ִ��� �˻�
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI ���� ���콺�� �ִٸ� ����ĳ������ �����ϰ� ����

                // �� ���� ��� �ൿ
                Execute();
                CheckUndoButtonStatus();
                Logger.Debug("�� 2�� ����? ");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                // UI ���� ���콺�� �ִ��� �˻�
                if (EventSystem.current.IsPointerOverGameObject())
                    return; // UI ���� ���콺�� �ִٸ� ����ĳ������ �����ϰ� ����

                // ���콺�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                // Ÿ�ϸʿ����� Ŭ�� ��ġ ���
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
            Logger.Debug("��");
        }
    }
    public void AwTestFunc(InputAction.CallbackContext context)
    {

        var dir = context.ReadValue<float>();


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

    public void Execute()// ����
    {
        if (undoStack.Count != 0)
        {
            undoStack.Clear();
        }

        // ���콺�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Ÿ�ϸʿ����� Ŭ�� ��ġ ���
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

                // GameObjectTileData �ν��Ͻ��� ����ϴ�.
                GameObjectTileData data = new GameObjectTileData();
                data.gameObject = hitCollider.gameObject;  // ���� ������Ʈ ����
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

    public void Undo()//���� ���
    {
        if (commandStack.Count > 0)
        {
            List<GameObjectTileData> gameObjList = commandStack.Pop();

            foreach (var gameObj in gameObjList)
            {
                if (gameObj.gameObject.activeSelf)
                {
                    // GameObject ��Ȱ��ȭ
                    gameObj.gameObject.SetActive(false);
                }
                else
                {
                    gameObj.gameObject.SetActive(true);
                }
            }

            // GameObject�� undoStack�� push
            undoStack.Push(gameObjList);
        }
        else
        {
            Debug.LogError("Nothing to undo.");
        }
        CheckUndoButtonStatus();
    }

    public void Redo()//�ٽ�
    {
        if (undoStack.Count > 0)
        {
            List<GameObjectTileData> gameObjList = undoStack.Pop();

            foreach (var gameObj in gameObjList)
            {
                if (gameObj.gameObject.activeSelf)
                {
                    // GameObject ��Ȱ��ȭ
                    gameObj.gameObject.SetActive(false);
                }
                else
                {
                    gameObj.gameObject.SetActive(true);
                }
            }

            // GameObject�� redoStack�� push
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
        // ���콺�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //// Ÿ�ϸʿ����� Ŭ�� ��ġ ���
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

                // GameObjectTileData �ν��Ͻ��� ����ϴ�.
                GameObjectTileData data = new GameObjectTileData();
                data.gameObject = hitCollider.gameObject;  // ���� ������Ʈ ����
                gameObjectTileDatas.Add(data);

                // GameObject�� undoStack�� push
                commandStack.Push(gameObjectTileDatas);
            }
        }
    }



}
