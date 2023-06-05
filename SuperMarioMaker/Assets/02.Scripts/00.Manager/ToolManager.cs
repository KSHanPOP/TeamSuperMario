using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField]
    private float playTime = 500f;
    [SerializeField]
    private int life = 3;
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
    [SerializeField]
    private int tilemapCol = 14;

    [SerializeField]
    private int tilemapStartline = 7;
    [SerializeField]
    private int tilemapEndline = 10;

    private bool[,] isDefalt;

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
}