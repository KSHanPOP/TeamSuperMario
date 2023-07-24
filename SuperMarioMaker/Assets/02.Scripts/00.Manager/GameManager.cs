using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    public float nowPlayTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        gameData = new GameData();
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
    public float Time;
    public int Life;
    public string BackGround;

    public int MapRowLength;
    public int MapColLength;

    public int TileX;
    public int TileY;
}