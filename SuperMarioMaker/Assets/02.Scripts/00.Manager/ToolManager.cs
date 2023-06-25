using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public enum ToolModeType
    {
        None,
        Tool,
        Test,
        Save,
    }

    private ToolModeType modeState = ToolModeType.None;
    public ToolModeType ToolMode
    {
        get { return modeState; }
        protected set
        {
            var prevState = modeState;

            modeState = value;

            if (prevState == modeState)
                return;
            switch (value)
            {
                case ToolModeType.None:

                    break;

                case ToolModeType.Tool:
                    modeState = ToolModeType.Tool;
                    break;

                case ToolModeType.Test:
                    modeState = ToolModeType.Test;
                    break;

                case ToolModeType.Save:
                    modeState = ToolModeType.Save;
                    break;
            }
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
    [SerializeField] Button topOnOffButton;
    [SerializeField] Button leftOnOffButton;
    [SerializeField] Button RightOnOffButton;
    [SerializeField] BarOnOffButton topOnOffButtonState;
    [SerializeField] BarOnOffButton leftOnOffButtonState;
    [SerializeField] BarOnOffButton RightOnOffButtonState;
    [SerializeField] GameObject toolUi;


    private void Init()
    {
        startButton.onClick.AddListener(AllUiOff);
    }

    public void AllUiOff()
    {
        if (!topOnOffButtonState.IsOnOff)
            topOnOffButton.onClick.Invoke();
        if (!leftOnOffButtonState.IsOnOff)
            leftOnOffButton.onClick.Invoke();
        if (!RightOnOffButtonState.IsOnOff)
            RightOnOffButton.onClick.Invoke();

        StartCoroutine(DeactivateAfterDelay(1f));
    }
    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        toolUi.SetActive(false);
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
