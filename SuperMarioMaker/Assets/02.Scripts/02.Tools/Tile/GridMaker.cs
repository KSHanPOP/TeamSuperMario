using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera miniMapCamera;

    [SerializeField] private Button onButton;
    [SerializeField] private Button offButton;

    [SerializeField] private GameObject popUp;

    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    private Vector3Int flagPos;// = new Vector3Int(-1, -1, 0);
    private List<Vector3Int> endLinePos = new List<Vector3Int>();

    public void PopupOn()
    {
        popUp.SetActive(true);
        onButton.gameObject.SetActive(false);
    }
    public void PopupOff()
    {
        onButton.gameObject.SetActive(true);
        popUp.SetActive(false);
    }
    public void SetDefaultTile()
    {
        int x = ToolManager.Instance.TilemapX;
        int y = ToolManager.Instance.TilemapY;
        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        CustomTile customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                tilemap.SetTile(cellPos, customTile);
            }
        }

        SetStartlineTile();
        SetEndlineTile();
        SetMinimapPosition();
    }
    public void SetStartlineTile()
    {
        int tilemapStartline = ToolManager.Instance.Startline;

        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        // CustomTile customTile = new CustomTile();
        CustomTile customTile = Resources.Load<CustomTile>("Sprite/TileSet/StaticTile/StaticTile_00");

        for (int i = 0; i < tilemapStartline; i++)
        {
            cellPos.x = i;
            tilemap.SetTile(cellPos, customTile);
        }
    }


    public void SetEndlineTile()
    {
        int tilemapEndline = ToolManager.Instance.TilemapX * ToolManager.Instance.MapRowLength;
        int startEndLine = tilemapEndline - ToolManager.Instance.Endline;

        //CustomTile customTile = new CustomTile();
        Vector3Int cellPos = new Vector3Int(startEndLine, 0, 0);

        CustomTile customTile = ScriptableObject.CreateInstance<CustomTile>();
        customTile = Resources.Load<CustomTile>("Sprite/TileSet/StaticTile/StaticTile_00");

        endLinePos.Clear();

        for (int i = startEndLine; i < tilemapEndline; i++)
        {
            cellPos.x = i;
            endLinePos.Add(cellPos);
            tilemap.SetTile(cellPos, customTile);
        }

        // 깃발
        customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        if (flagPos != Vector3Int.zero)
            tilemap.SetTile(flagPos, customTile);

        customTile = Resources.Load<CustomTile>("Sprite/TileSet/StaticTile/StaticTile_11");

        cellPos.x = startEndLine;
        cellPos.y = 1;

        tilemap.SetTile(cellPos, customTile);

        flagPos = cellPos;
    }

    public void RightGrid()
    {
        if (!ToolManager.Instance.SetIncreaseMapRowLength())
            return;

        int beforeRow = ToolManager.Instance.MapRowLength - 1;
        int nowkRow = ToolManager.Instance.MapRowLength;

        int startPosX = ToolManager.Instance.TilemapX * beforeRow;
        int endPosX = ToolManager.Instance.TilemapX * nowkRow;

        int y = ToolManager.Instance.TilemapY * ToolManager.Instance.MapColLength;
        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        CustomTile customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        //기존 엔드라인 삭제
        foreach (Vector3Int EndLinePos in endLinePos)
        {
            tilemap.SetTile(EndLinePos, customTile);
        }


        for (int i = startPosX; i < endPosX; i++)
        {
            for (int j = 0; j < y; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                tilemap.SetTile(cellPos, customTile);
            }
        }

        SetEndlineTile();
        SetMinimapPosition();
    }
    public void LeftGrid()
    {
        if (ToolManager.Instance.MapRowLength == 1)
            return;

        if (!ToolManager.Instance.SetDecreaseMapRowLength())
            return;


        SetEndlineTile();


        int beforeRow = ToolManager.Instance.MapRowLength;
        int nowkRow = ToolManager.Instance.MapRowLength + 1;

        int startPosX = ToolManager.Instance.TilemapX * beforeRow;
        int endPosX = ToolManager.Instance.TilemapX * nowkRow;

        int y = ToolManager.Instance.TilemapY * ToolManager.Instance.MapColLength;
        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        //CustomTile customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        //기존 엔드라인 삭제
        //foreach (Vector3Int EndLinePos in endLinePos)
        //{
        //    tilemap.SetTile(EndLinePos, null);
        //}


        for (int i = startPosX; i < endPosX; i++)
        {
            for (int j = 0; j < y; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                tilemap.SetTile(cellPos, null);
            }
        }

        SetMinimapPosition();
    }
    public void UpGrid()
    {
        if (!ToolManager.Instance.SetIncreaseMapColLength())
            return;

        int beforeCol = ToolManager.Instance.MapColLength - 1;
        int nowkCol = ToolManager.Instance.MapColLength;

        int startPosY = ToolManager.Instance.TilemapY * beforeCol;
        int endPosY = ToolManager.Instance.TilemapY * nowkCol;

        int x = ToolManager.Instance.TilemapX * ToolManager.Instance.MapRowLength;

        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        CustomTile customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        for (int i = 0; i < x; i++)
        {
            for (int j = startPosY; j < endPosY; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                tilemap.SetTile(cellPos, customTile);
            }
        }

        SetMinimapPosition();
    }

    public void DownGrid()
    {
        if (ToolManager.Instance.MapColLength == 1)
            return;

        if (!ToolManager.Instance.SetDecreaseMapColLength())
            return;

        int beforeCol = ToolManager.Instance.MapColLength;
        int nowkCol = ToolManager.Instance.MapColLength + 1;

        int startPosY = ToolManager.Instance.TilemapY * beforeCol;
        int endPosY = ToolManager.Instance.TilemapY * nowkCol;

        int x = ToolManager.Instance.TilemapX * ToolManager.Instance.MapRowLength;

        Vector3Int cellPos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < x; i++)
        {
            for (int j = startPosY; j < endPosY; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                tilemap.SetTile(cellPos, null);
            }
        }

        SetMinimapPosition();
    }

    public void SetMinimapPosition()
    {
        tilemap.CompressBounds();
        Vector3 tilemapCenter = tilemap.cellBounds.center;
        miniMapCamera.transform.position = new Vector3(tilemapCenter.x, tilemapCenter.y, miniMapCamera.transform.position.z);

        float sizeX = tilemap.cellBounds.size.x / (2.0f * Mathf.Abs(miniMapCamera.aspect));
        float sizeY = tilemap.cellBounds.size.y / 2.0f;

        miniMapCamera.orthographicSize = Mathf.Max(sizeX, sizeY);
    }
    public void Init()
    {

        rightButton.onClick.AddListener(() => RightGrid());
        leftButton.onClick.AddListener(() => LeftGrid());
        upButton.onClick.AddListener(() => UpGrid());
        downButton.onClick.AddListener(() => DownGrid());

        SetDefaultTile();

        onButton.onClick.AddListener(() => PopupOn());
        offButton.onClick.AddListener(() => PopupOff());
        popUp.SetActive(false);
    }
    void Start()
    {
        Init();
    }

    void Update()
    {

    }
}
