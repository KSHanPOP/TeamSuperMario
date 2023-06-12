using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IconManager iconManager;
    public ScrollingBackground Background;

    [SerializeField]
    private float playTime = 500f;
    public float PlayTime
    { get { return playTime; } set { playTime = value; } }

    [SerializeField]
    private int playerLife = 3;
    public int PlayerLife
    { get { return playerLife; } set { playerLife = value; } }

    [SerializeField]
    private string background = "Background Ground";

    public string BackgroundName
    {
        get { return background; }
        set { background = value; }
    }

    [SerializeField]
    [Range(1, 10)]
    private int mapRowLength = 1;
    public int MapRowLength
    { get { return mapRowLength; } set { mapRowLength = value; } }

    [SerializeField]
    [Range(1, 10)]
    private int mapColLength = 1;
    public int MapColLength
    { get { return mapColLength; } set { mapColLength = value; } }

    public int maxMapLength = 11;

    [SerializeField]
    private int tilemapRow = 24;
    public int TilemapX
    { get { return tilemapRow; } }

    [SerializeField]
    private int tilemapCol = 14;
    public int TilemapY
    { get { return tilemapCol; } }

    [SerializeField]
    private int tilemapStartline = 7;
    public int Startline { get { return tilemapStartline; } }


    [SerializeField]
    private int tilemapEndline = 10;
    public int Endline { get { return tilemapEndline; } }

    private bool[,] isDefalt;

    private Vector3 playerPos;
    public struct DefaultInfo
    {
        public Vector3Int pos;
    }

    public bool CheckMaxMapLength() => MapRowLength + mapColLength + 1 <= maxMapLength;
    public bool CheckMinMapLength() => MapRowLength + mapColLength - 1 >= 2;

    public void SetIncreaseMapRowLength()
    {
        if (!CheckMaxMapLength())
            return;

        ++mapRowLength;
    }
    public void SetDecreaseMapRowLength()
    {
        if (!CheckMinMapLength())
            return;

        --mapRowLength;
    }
    public void SetIncreaseMapColLength()
    {
        if (!CheckMaxMapLength())
            return;

        ++mapColLength;
    }
    public void SetDecreaseMapColLength()
    {
        if (!CheckMinMapLength())
            return;

        --mapColLength;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
