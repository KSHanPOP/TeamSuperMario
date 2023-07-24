using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance { get; private set; }

    private bool isStart;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject Select;

    [SerializeField] private Button gameStart;
    [SerializeField] private Button maker;
    [SerializeField] private Button sound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }

        if (SaveLoadManager.Instance != null)
        {
            loadPopup = SaveLoadManager.Instance.PopUp;
        }
        else
        {
            Debug.LogError("SaveLoadManager instance not found");
        }
    }
    void Start()
    {
        gameStart.onClick.AddListener(OnGameStart);
        maker.onClick.AddListener(ToolStart);
        sound.onClick.AddListener(OnSound);
        Init();
    }

    public void Init()
    {
        isStart = false;
        start.SetActive(true);
        Select.SetActive(false);
    }

    void Update()
    {
        if (!isStart && Input.anyKeyDown)
        {
            isStart = true;

            start.SetActive(false);
            Select.SetActive(true);
        }
    }

    [SerializeField] private GameObject loadPopup;
    public void OnGameStart()
    {
        loadPopup.SetActive(true);
    }

    public void ToolStart()
    {
        SceneLoader.Instance.LoadToolScene();
    }
    public void OnSound()
    {
        SoundManager.Instance.OnSoundPopup();
    }
}
