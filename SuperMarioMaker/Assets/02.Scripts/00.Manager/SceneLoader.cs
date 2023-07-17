using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private GameObject loadingBG;

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

    private SceneState state;
    public SceneState State
    {
        get { return state; }
        set
        {
            state = value;

            switch (state)
            {
                case SceneState.Title:
                    TitleManager.Instance.Init();
                    LoadTitleScene();
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
        SceneManager.LoadScene("Title");
    }

    public void LoadToolScene()
    {
        //SceneManager.LoadScene("Tool");
        StartCoroutine(LoadSceneAndInit("Tool"));
    }

    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("MainGame");
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
                SoundManager.Instance.PlayBGM("Title");
                break;
            case "Tool":
                break;
            case "MainGame":
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
