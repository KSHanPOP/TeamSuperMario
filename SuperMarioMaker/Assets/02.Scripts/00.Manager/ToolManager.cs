using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }


    public JsonTest CJsonTest;

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

        BackgroundName = "Background Ground";
    }

    public enum ToolModeType
    {
        None,
        Tool,
        Test,
        Save,
    }

    public ClickChangeTile CClickMode;

    [SerializeField] ScrollingBackground scrollingBackground;

    private ToolModeType modeState = ToolModeType.Tool;
    public ToolModeType ToolMode
    {
        get { return modeState; }
        protected set
        {
            var prevState = modeState;

            modeState = value;
            scoreText.text = "000000000";
            coinText.text = "00";

            if (prevState == modeState)
                return;
            switch (value)
            {
                case ToolModeType.None:

                    break;

                case ToolModeType.Tool:
                    StopCoroutine(timeCoroutine);
                    toolUi.SetActive(true);
                    testModeUi.SetActive(false);

                    if (topOnOffButtonState.IsOnOff)
                        topOnOffButton.onClick.Invoke();
                    if (leftOnOffButtonState.IsOnOff)
                        leftOnOffButton.onClick.Invoke();
                    if (RightOnOffButtonState.IsOnOff)
                        RightOnOffButton.onClick.Invoke();
                    scrollingBackground.StopBackground();
                    TileManager.Instance.StopTest();

                    iconManager.NowTag = "None";

                    SoundManager.Instance.StopAll();
                    SoundManager.Instance.PlayBGM("Tool");

                    break;

                case ToolModeType.Test:
                    if (!topOnOffButtonState.IsOnOff)
                        topOnOffButton.onClick.Invoke();
                    if (!leftOnOffButtonState.IsOnOff)
                        leftOnOffButton.onClick.Invoke();
                    if (!RightOnOffButtonState.IsOnOff)
                        RightOnOffButton.onClick.Invoke();
                    SetGameData();
                    nowLife = PlayerLife;
                    StartCoroutine(DeactivateAfterDelay(1f));
                    break;

                case ToolModeType.Save:
                    if (!topOnOffButtonState.IsOnOff)
                        topOnOffButton.onClick.Invoke();
                    if (!leftOnOffButtonState.IsOnOff)
                        leftOnOffButton.onClick.Invoke();
                    if (!RightOnOffButtonState.IsOnOff)
                        RightOnOffButton.onClick.Invoke();
                    SetGameData();
                    nowLife = PlayerLife;
                    StartCoroutine(DeactivateAfterDelay(1f));
                    break;
            }
        }
    }

    public string BgNameToSound(string bgName)
    {
        string[] words = bgName.Split(' ');
        if (words.Length > 1)
        {
            return words[1];
        }
        else
        {
            return string.Empty;
        }
    }

    public IconManager iconManager;
    public GridMaker gridMaker;

    private GameObject playerObj;
    public GameObject PlayerObj
    {
        get { return playerObj; }
        set { playerObj = value; }
    }

    [SerializeField]
    private float playTime = 500f;
    public float PlayTime
    { get { return playTime; } set { playTime = value; } }

    [SerializeField]
    private int playerLife = 3;
    public int PlayerLife
    { get { return playerLife; } set { playerLife = value; } }

    [SerializeField]
    private string background;

    public string BackgroundName
    {
        get { return background; }
        set { background = value; }
    }
    void SetGameData()
    {
        GameData data = GameManager.instance.gameData;
        data.Time = playTime;
        data.Life = playerLife;
        data.BackGround = background;
        data.MapRowLength = MapRowLength;
        data.TileX = TilemapX; data.TileY = TilemapY;
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

    public bool SetIncreaseMapRowLength()
    {
        if (!CheckMaxMapLength())
            return false;

        ++mapRowLength;

        return true;
    }
    public bool SetDecreaseMapRowLength()
    {
        if (!CheckMinMapLength())
            return false;

        --mapRowLength;
        return true;
    }
    public bool SetIncreaseMapColLength()
    {
        if (!CheckMaxMapLength())
            return false;

        ++mapColLength;

        return true;
    }
    public bool SetDecreaseMapColLength()
    {
        if (!CheckMinMapLength())
            return false;

        --mapColLength;
        return true;
    }

    [SerializeField] Button startButton;
    [SerializeField] Button saveButton;
    [SerializeField] Button topOnOffButton;
    [SerializeField] Button leftOnOffButton;
    [SerializeField] Button RightOnOffButton;
    [SerializeField] BarOnOffButton topOnOffButtonState;
    [SerializeField] BarOnOffButton leftOnOffButtonState;
    [SerializeField] BarOnOffButton RightOnOffButtonState;
    [SerializeField] GameObject toolUi;

    [SerializeField] Button stopButton;
    [SerializeField] GameObject testModeUi;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI scoreText;

    private int nowLife;

    private void Init()
    {
        startButton.onClick.AddListener(GoTest);
        stopButton.onClick.AddListener(GoTool);
        saveButton.onClick.AddListener(GoSave);

        SoundManager.Instance.PlayBGM("Tool");
    }

    public void GoTest() => ToolMode = ToolModeType.Test;
    public void GoTool() => ToolMode = ToolModeType.Tool;
    public void GoSave() => ToolMode = ToolModeType.Save;

    private Coroutine timeCoroutine;
    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        toolUi.SetActive(false);
        testModeUi.SetActive(true);
        timeCoroutine = StartCoroutine(TimeUpdateCoroutine());
        scrollingBackground.MoveBackground();
        TileManager.Instance.StartTest();
        BgNameToSound(BackgroundName);
    }
    private IEnumerator TimeUpdateCoroutine()
    {
        var time = PlayTime;
        while (true)
        {
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1.0f);
            time--;
            GameManager.instance.nowPlayTime = time;
            if (time < 0)
            {
                PlayerState.Instance.CurrState.Die();
                yield break;
            }
        }
    }

    public void AddScore(int score)
    {
        int nowscore = int.Parse(scoreText.text);
        nowscore += score;

        scoreText.text = nowscore.ToString("D8");
    }

    public void AddCoin(int coin)
    {
        int nowCoin = int.Parse(coinText.text);

        nowCoin += coin;

        if (nowCoin == 100)
            nowCoin = 1;

        coinText.text = nowCoin.ToString("D2");
    }

    public MapData SetMapData()
    {
        MapData data = new MapData();

        data.MapName = DateTime.Now.ToString("HH:mm:ss");
        data.Time = PlayTime;
        data.Life = PlayerLife;

        data.BackGroundName = BackgroundName;
        data.MapRowLength = MapRowLength;
        data.MapColLength = MapColLength;

        data.Tiles = new List<TileData>();

        data.Tiles.AddRange(gridMaker.SaveData());
        data.Tiles.AddRange(CClickMode.SaveData());

        return data;
    }

    void Start()
    {

        Logger.CheckNullObject(this);
        Init();
    }

    void Update()
    {

    }

}
