using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform cameraFrameRect;
    [SerializeField] private GameObject miniMapCanvas;


    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera miniMapCamera;

    [SerializeField] private Button onButton;
    [SerializeField] private Button offButton;

    [SerializeField] private GameObject popUp;

    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    [SerializeField] private TextMeshProUGUI upText;
    [SerializeField] private TextMeshProUGUI downText;

    [SerializeField] private ScrollingBackground background;

    private Vector3Int flagPos;// = new Vector3Int(-1, -1, 0);
    private List<Vector3Int> endLinePos = new List<Vector3Int>();

    private Dictionary<Vector3Int, GameObjectTileData> DicTileData = new Dictionary<Vector3Int, GameObjectTileData>();

    private void SetDefaultObj(string objectName, Vector3Int pos)
    {
        var newObject = ResourceManager.instance.GetSpawnPrefabByName(objectName, pos);

        // Create a new TileData object
        TileData newTileData = new TileData
        {
            X = pos.x,
            Y = pos.y,
            TileType = "defalut",
            TileName = objectName,
            // Add other necessary properties here
        };

        // GameObjectTileData 생성 및 초기화
        GameObjectTileData newGameObjectTileData = new GameObjectTileData
        {
            gameObject = newObject,
            tileData = newTileData,
        };

        // Check if the key already exists
        if (!DicTileData.ContainsKey(pos))
        {
            // Add the new TileData to the dictionary
            DicTileData.Add(pos, newGameObjectTileData);
        }
        else
        {
            // If the key already exists, update the value
            DicTileData[pos] = newGameObjectTileData;
        }
    }
    private void EraseDefaultObj(Vector3Int pos)
    {
        if (DicTileData.ContainsKey(pos))
        {
            // 좌표가 있다면 GameObject 삭제
            GameObject toDestroy = DicTileData[pos].gameObject;
            Destroy(toDestroy);

            // 딕셔너리에서 정보 제거
            DicTileData.Remove(pos);
        }
        else
        {
            Logger.Debug("Erase Data Fail");
        }
    }
    private void CheckedNull()
    {

    }
    public void PopupOn()
    {
        popUp.SetActive(true);
        //onButton.gameObject.SetActive(false);
    }
    public void PopupOff()
    {
        //onButton.gameObject.SetActive(true);
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
            SetDefaultObj("Platform", cellPos);

            //tilemap.SetTile(cellPos, customTile);
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
            SetDefaultObj("Platform", cellPos);

            //tilemap.SetTile(cellPos, customTile);
        }

        // 깃발
        customTile = Resources.Load<CustomTile>("Sprite/TileSet/Grid");

        if (flagPos != Vector3Int.zero)
        {
            // 깃발 지우기
            EraseDefaultObj(flagPos);

            // 캐슬 지우기
            var casltePos = flagPos;
            casltePos.x += 7;
            EraseDefaultObj(casltePos);
            tilemap.SetTile(flagPos, customTile);
        }

        // customTile = Resources.Load<CustomTile>("Sprite/TileSet/StaticTile/StaticTile_11");

        cellPos.x = startEndLine;
        cellPos.y = 1;

        SetDefaultObj("Goal", cellPos);
        var CasltePos = cellPos;
        CasltePos.x += 7;
        SetDefaultObj("Caslte", CasltePos);

        //tilemap.SetTile(cellPos, customTile);

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
            EraseDefaultObj(EndLinePos);

        }


        for (int i = startPosX; i < endPosX; i++)
        {
            for (int j = 0; j < y; j++)
            {
                cellPos.x = i;
                cellPos.y = j;
                EraseDefaultObj(cellPos);
                tilemap.SetTile(cellPos, customTile);
            }
        }

        SetEndlineTile();
        SetMinimapPosition();
        SetTextVelue(nowkRow, false);
        background.SetSizeChange();
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
                EraseDefaultObj(cellPos);
                tilemap.SetTile(cellPos, null);
            }
        }

        SetMinimapPosition();
        SetTextVelue(beforeRow, false);
        background.SetSizeChange();

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
        SetTextVelue(nowkCol, true);
        background.SetSizeChange();

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
        SetTextVelue(beforeCol, true);
        background.SetSizeChange();

    }

    public void SetMinimapPosition()
    {
        tilemap.CompressBounds();
        Vector3 tilemapCenter = tilemap.cellBounds.center;
        miniMapCamera.transform.position = new Vector3(tilemapCenter.x, tilemapCenter.y, miniMapCamera.transform.position.z);

        float sizeX = tilemap.cellBounds.size.x / (2.0f * Mathf.Abs(miniMapCamera.aspect));
        float sizeY = tilemap.cellBounds.size.y / 2.0f;

        miniMapCamera.orthographicSize = Mathf.Max(sizeX, sizeY);
        SetFrame();
    }

    public void SetFrame()
    {
        // 메인 카메라의 뷰포트 좌표 기준 하단 좌측과 우측 상단을 가져옵니다.
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.farClipPlane));

        // 미니맵 카메라가 볼 수 있는 전체 월드 좌표 영역을 계산합니다.
        float worldWidth = 2f * miniMapCamera.orthographicSize * miniMapCamera.aspect;
        float worldHeight = 2f * miniMapCamera.orthographicSize;

        // 미니맵 UI 요소의 픽셀 크기를 가져옵니다.
        RectTransform miniMapRect = miniMapCanvas.GetComponent<RectTransform>();
        float pixelWidth = miniMapRect.rect.width;
        float pixelHeight = miniMapRect.rect.height;

        // 월드 좌표와 픽셀 좌표 사이의 비율을 계산합니다.
        float pixelPerUnitX = pixelWidth / worldWidth;
        float pixelPerUnitY = pixelHeight / worldHeight;

        // 메인 카메라의 월드 좌표 영역을 미니맵의 픽셀 좌표로 변환합니다.
        Vector2 bottomLeftPixel = new Vector2(bottomLeft.x * pixelPerUnitX, bottomLeft.y * pixelPerUnitY);
        Vector2 topRightPixel = new Vector2(topRight.x * pixelPerUnitX, topRight.y * pixelPerUnitY);

        // 이제 이 좌표를 사용하여 UI 요소의 위치와 크기를 설정할 수 있습니다.
        Vector2 position = bottomLeftPixel;
        Vector2 size = topRightPixel - bottomLeftPixel;

        // UI 요소의 위치와 크기를 업데이트합니다.
        cameraFrameRect.anchoredPosition = position;
        cameraFrameRect.sizeDelta = size;
    }

    public void SetTextVelue(int value, bool up)
    {
        if (up)
        {
            upText.text = value.ToString();
        }
        else
        {
            downText.text = value.ToString();
        }
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
