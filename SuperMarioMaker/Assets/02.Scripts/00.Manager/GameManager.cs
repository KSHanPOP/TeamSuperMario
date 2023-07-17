using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}

public class GameData
{
    public float Time { get { return Time; } set { Time = value; } }
    public int Life { get { return Life; } set { Life = value; } }
    public string BackGround { get { return BackGround; } set { BackGround = value; } }

    public int MapRowLength { get { return MapColLength; } set { MapColLength = value; } }
    public int MapColLength { get { return MapRowLength; } set { MapColLength = value; } }

    public int TileX { get { return TileX; } set { TileX = value; } }
    public int TileY { get { return TileY; } set { TileY = value; } }
}