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

    [SerializeField]
    private float playTime = 500f;
    [SerializeField]
    private int playerLife = 3;
    [SerializeField]
    private string background = "Ground";

    [SerializeField]
    [Range(1, 10)]
    private int mapRowLength = 1;

    [SerializeField]
    [Range(1, 10)]

    private int mapColLength = 1;

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

    [SerializeField]
    private int tilemapEndline = 10;

    private bool[,] isDefalt;

    private Vector3 playerPos;
    public struct DefaultInfo
    {
        public Vector3Int pos;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetPlayTime(float time) => playTime = time;
    public void SetLife(int life) => playerLife = life;


}
