using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMaker : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Vector3Int flagPos;// = new Vector3Int(-1, -1, 0);
    private List<Vector3Int> endLinePos;
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
        tilemap.CompressBounds();
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
        int tilemapEndline = ToolManager.Instance.TilemapX * ToolManager.Instance.MapColLength;
        int startEndLine = tilemapEndline - ToolManager.Instance.Endline;

        Vector3Int cellPos = new Vector3Int(startEndLine, 0, 0);

        CustomTile customTile = new CustomTile();
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

        if (flagPos != null)
            tilemap.SetTile(flagPos, customTile);

        customTile = Resources.Load<CustomTile>("Sprite/TileSet/StaticTile/StaticTile_11");

        cellPos.x = startEndLine;
        cellPos.y = 1;

        tilemap.SetTile(cellPos, customTile);

        flagPos = cellPos;
    }

    public void AddRow(int beforeRow, int nowkRow, int col)
    {
        int startPosX = ToolManager.Instance.TilemapX * beforeRow;
        int endPosX = ToolManager.Instance.TilemapX * nowkRow;

        int y = ToolManager.Instance.TilemapY * col;
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
        tilemap.CompressBounds();
    }

    public void Init()
    {
        SetDefaultTile();
    }
    void Start()
    {
        Init();
    }

    void Update()
    {

    }
}
