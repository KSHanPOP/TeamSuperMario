using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    private string fileName;

    private List<TileData> Tiles { get; set; }

    private GameData gameData;
    public GameData GameData { get { return gameData; } }

    private bool isStart = false;


    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI life;
    [SerializeField] private TextMeshProUGUI coin;
    private int iCoin = 0;
    [SerializeField] private TextMeshProUGUI point;
    private int iPoint = 0;

    [SerializeField] private Image blackOut;
    [SerializeField] private TextMeshProUGUI courseClearOrFail;

    [SerializeField] private MoveBackGround backGround;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        fileName = SceneLoader.Instance.NowGameName;
        SetGameData();
    }

    private void SetGameData()
    {
        MapData mapData = SaveLoadManager.Instance.SaveFiles[fileName].mapData;

        gameData = new GameData();

        gameData.Time = mapData.Time;
        gameData.Life = mapData.Life;
        gameData.BackGround = mapData.BackGroundName;

        gameData.MapRowLength = mapData.MapRowLength;
        gameData.MapColLength = mapData.MapColLength;

        Tiles = new List<TileData>(mapData.Tiles);

        SetGameDataInfo();
        ResetGameDataInfo();
        SetGameObject();
    }

    private void SetGameObject()
    {
        foreach (var tile in Tiles)
        {
            var pos = new Vector3(tile.X, tile.Y, 0f);
            var obj = ResourceManager.instance.GetSpawnPrefabByName(tile.TileName, pos);

            switch (tile.TileName)
            {
                case "Upward":
                case "Leftward":
                case "Rightward":
                case "Downward":
                    obj.GetComponent<Pipe>().SetValue(tile.TileValue1, tile.TileValue2);
                    break;
                case "Thwomp":
                    obj.GetComponent<DynamicTile_Thwomp>().SetValue(tile.TileValue1);
                    break;
                case "DynamicTile_Block":
                case "Question_Block":
                    obj.GetComponent<DynamicTile_Block>().SetValue(tile.TileValue1, tile.TileValue2);
                    break;
            }            
        }
    }

    private void SetGameDataInfo()
    {
        MapData mapData = SaveLoadManager.Instance.SaveFiles[fileName].mapData;

        life.text = mapData.Life.ToString("00");
    }
    private void ResetGameDataInfo()
    {
        MapData mapData = SaveLoadManager.Instance.SaveFiles[fileName].mapData;

        time.text = mapData.Time.ToString("000");
        count.text = "3";
        coin.text = "00";
        iCoin = 0;
        point.text = "00000000";
    }
    private void GameStart()
    {
        isStart = true;
        string[] words = gameData.BackGround.Split(' ');

        SoundManager.Instance.PlayBGM(words[1]);
        TileManager.Instance.StartGame();
        StartCoroutine(TimeCounter());

        backGround.StartMove();
    }
    public void Die()
    {
        AddLife(false);

        if (gameData.Life == 0)
        {
            CourseFail();
            return;
        }

        if (gameData.Time <= 0)
            PlayerState.Instance.CurrState.Die();

        SoundManager.Instance.StopAll();

        backGround.StopMove();
        StartCoroutine(FadeBlackOut());

        ResetGameDataInfo();
    }
    public void CourseClear()
    {
        courseClearOrFail.gameObject.SetActive(true);
        courseClearOrFail.text = "Course Clear";
        StartCoroutine(FadeBlackOut(3f));
    }
    private void CourseFail()
    {
        SoundManager.Instance.PlaySFX("gameover");
        courseClearOrFail.gameObject.SetActive(true);
        courseClearOrFail.text = "GameOver";

        StartCoroutine(FadeBlackOut(3f));
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        count.gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM("CountDown");

        for (int i = 3; i > 0; i--)
        {
            count.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        count.text = "Start!";
        yield return new WaitForSeconds(1);

        count.gameObject.SetActive(false);
        GameStart();
    }

    IEnumerator TimeCounter()
    {
        while (gameData.Time > 0)
        {
            if (!isStart)
                yield break;

            gameData.Time--;
            time.text = gameData.Time.ToString("000");
            yield return new WaitForSeconds(1);
        }
        Die();
    }

    [SerializeField] private float blackOutTime;
    [SerializeField] private float fadeTime;

    IEnumerator FadeBlackOut()
    {
        TileManager.Instance.StopGame();
        blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, 1);
        yield return new WaitForSeconds(blackOutTime);

        for (float t = fadeTime; t >= 0; t -= Time.deltaTime)
        {
            blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, t);
            yield return null;
        }
        StartCountDown();
    }

    IEnumerator FadeBlackOut(float blackOutTime)
    {
        blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, 1);
        yield return new WaitForSeconds(blackOutTime);

        SceneLoader.Instance.LoadTitleScene();
    }

    public void PointCalculate() => StartCoroutine(AddRemainingTimeAsPoints());
    IEnumerator AddRemainingTimeAsPoints()
    {
        float delay = 2f / gameData.Time; // Delay for each point increment.
        float temp = 0f;

        while (gameData.Time > 0)
        {
            AddTime(-1);
            AddPoint(100);

            temp += delay;
            if (temp > 0.05f)
            {
                SoundManager.Instance.PlaySFX("Coin");
                temp = 0f;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public void AddCoin(int addCoin)
    {
        if (iCoin == 99)
        {
            iCoin = 0;
            AddLife(true);
        }
        else
        {
            iCoin += addCoin;
            coin.text = iCoin.ToString("00");
        }
    }
    public void AddPoint(int addPoint)
    {
        iPoint += addPoint;
        point.text = iPoint.ToString("00000000");
    }
    public void AddLife(bool addLife)
    {
        if (addLife)
        {
            gameData.Life += 1;
        }
        else
        {
            gameData.Life -= 1;
        }
        life.text = gameData.Life.ToString("00");
    }
    public void AddTime(float addTime)
    {
        gameData.Time += addTime;

        time.text = gameData.Time.ToString("000");
    }
    public void TimeStop()
    {
        isStart = false;
    }
}
