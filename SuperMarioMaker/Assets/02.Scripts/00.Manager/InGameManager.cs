using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    private string fileName;

    private List<TileData> Tiles { get; set; }

    private GameData gameData;

    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI life;
    [SerializeField] private TextMeshProUGUI coin;
    private int iCoin = 0;
    [SerializeField] private TextMeshProUGUI point;
    private int iPoint = 0;

    [SerializeField] private Image blackOut;
    [SerializeField] private TextMeshProUGUI courseClearOrFail;

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
            ResourceManager.instance.SpawnPrefabByName(tile.TileName, pos);
        }
    }

    private void SetGameDataInfo()
    {
        MapData mapData = SaveLoadManager.Instance.SaveFiles[fileName].mapData;

        time.text = mapData.Time.ToString("000");
        life.text = mapData.Life.ToString("00");
    }
    private void ResetGameDataInfo()
    {
        count.text = "3";
        coin.text = "00";
        iCoin = 0;
        point.text = "00000000";
    }
    private void GameStart()
    {
        string[] words = gameData.BackGround.Split(' ');

        SoundManager.Instance.PlayBGM(words[1]);

        TimeCounter();
    }
    private void Die()
    {
        AddLife(false);

        if (gameData.Life == 0)
        {
            CourseFail();
        }

        SoundManager.Instance.StopAll();
        StartCoroutine(FadeBlackOut());

        ResetGameDataInfo();
    }
    private void CourseClear()
    {
        courseClearOrFail.gameObject.SetActive(true);
        courseClearOrFail.text = "Course Clear";
        StartCoroutine(FadeBlackOut(3f));

    }
    private void CourseFail()
    {
        courseClearOrFail.gameObject.SetActive(true);
        courseClearOrFail.text = "Fail";
        StartCoroutine(FadeBlackOut(3f));
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
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
        blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, 1);
        yield return new WaitForSeconds(blackOutTime);

        for (float t = fadeTime; t >= 0; t -= Time.deltaTime)
        {
            blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, t);
            yield return null;
        }


    }

    IEnumerator FadeBlackOut(float blackOutTime)
    {
        blackOut.color = new Color(blackOut.color.r, blackOut.color.g, blackOut.color.b, 1);
        yield return new WaitForSeconds(blackOutTime);

        SceneLoader.Instance.LoadTitleScene();
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
        coin.text = gameData.Life.ToString("00");
    }
}
