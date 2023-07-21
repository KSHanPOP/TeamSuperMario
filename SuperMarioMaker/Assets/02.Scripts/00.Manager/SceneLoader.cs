using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private GameObject loadingBG;
    [SerializeField] private Button exitButton;

    private string nowGameName;
    public string NowGameName
    {
        get { return nowGameName; }
        private set { nowGameName = value; }
    }

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

        SoundManager.Instance.PlayBGM("Title");
    }
    private void Start()
    {
        exitButton.onClick.AddListener(LoadTitleScene);
        state = SceneState.Title;
        SoundManager.Instance.PlayBGM("Title");
    }

    //public void GoToTitle()
    //{
    //    State = SceneState.Title;
    //}

    private SceneState state;
    public SceneState State
    {
        get { return state; }
        set
        {
            SoundManager.Instance.StopAll();
            SoundManager.Instance.AllPopUpOff();

            state = value;

            switch (state)
            {
                case SceneState.Title:

                    break;
                case SceneState.Tool:

                    break;
                case SceneState.MainGame:
                    break;
            }
        }
    }

    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAndInit("Title"));
    }


    public void LoadToolScene()
    {
        StartCoroutine(LoadSceneAndInit("Tool"));
    }

    public void LoadMainGameScene(string gameName)
    {
        NowGameName = gameName;
        StartCoroutine(LoadSceneAndInit("MainGame"));
    }

    private IEnumerator LoadSceneAndInit(string sceneName)
    {
        loadingBG.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        switch (sceneName)
        {
            case "Title":
                SoundManager.Instance.BGMStop();
                State = SceneState.Title;
                break;
            case "Tool":
                SoundManager.Instance.BGMStop();
                State = SceneState.Tool;
                break;
            case "MainGame":
                SoundManager.Instance.BGMStop();
                State = SceneState.MainGame;
                break;
        }
        // Scene has loaded, now you can access new scene objects

        yield return new WaitForSeconds(3);
        loadingBG.SetActive(false);
        switch (sceneName)
        {
            case "Title":
                TitleManager.Instance.Init();
                SoundManager.Instance.PlayBGM("Title");
                break;
            case "Tool":
                SoundManager.Instance.PlayBGM("Tool");
                break;
            case "MainGame":
                InGameManager.Instance.StartCountDown();
                break;
        }
    }
}


public enum SceneState
{
    Title,
    Tool,
    MainGame,
}
