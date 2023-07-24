using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private GameObject icon;

    private Coroutine backgroundCoroutine;

    [SerializeField]
    private Button upButton;
    [SerializeField]
    private Button downButton;

    [SerializeField] private GameObject tempObject;
    GameObject[,] backgroundImages;

    private float mapRowLength;
    private float mapColLength;
    private string backGroundName;
    private string backGroundName1;

    private float spriteWidth;

    public float scrollSpeed;
    private int x;
    private int y;

    private void Awake()
    {
        spriteWidth = Resources.Load<Sprite>("Sprite/Backgrounds/Background Ground").bounds.size.x;
    }
    void Start()
    {
        Logger.CheckNullObject(this);

        LoadBackgroundSprites();
        InitializeBackgrounds();
        Init();
        SetImages();
    }
    private void Init()
    {
        //foreach (Transform background in backgrounds)
        //{
        //    initialPositions[background] = background.position;
        //}

        SaveBackgroundPositions();

        upButton.onClick.AddListener(BackgroundImageUp);
        downButton.onClick.AddListener(BackgroundImageDown);
    }
    void SaveBackgroundPositions()
    {
        backgroundImages = new GameObject[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                backgroundImages[i, j] = Instantiate(tempObject);
                backgroundImages[i, j].transform.position
                    = new Vector3(spriteWidth * i, spriteWidth * j, 0f);
            }
        }
    }
    void InitializeBackgrounds()
    {
        mapRowLength = ToolManager.Instance.MapRowLength * 24;
        mapColLength = ToolManager.Instance.MapColLength * 13.5f;

        backGroundName = ToolManager.Instance.BackgroundName;
        backGroundName1 = backGroundName + "1";

        CalculateNumberOfBackgroundImages();
    }
    private void CalculateNumberOfBackgroundImages()
    {
        x = Mathf.CeilToInt(mapRowLength / spriteWidth) + 1;
        y = Mathf.CeilToInt(mapColLength / spriteWidth);
    }

    private List<Sprite> backgroundSprites;
    private void LoadBackgroundSprites()
    {
        backgroundSprites = new List<Sprite>();

        // Resources/Background 폴더에서 모든 스프라이트를 로드합니다.
        object[] loadedSprites = Resources.LoadAll("Background", typeof(Sprite));

        foreach (Sprite sprite in loadedSprites)
        {
            backgroundSprites.Add(sprite);
        }
    }



    public void SetSizeChange()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (backgroundImages[i, j] != null)
                {
                    Destroy(backgroundImages[i, j].gameObject);
                }
            }
        }

        InitializeBackgrounds();
        SaveBackgroundPositions();
        SetImages();
    }
    private void Update()
    {

    }
    public void MoveBackground()
    {
        if (backgroundCoroutine == null)
        {
            backgroundCoroutine = StartCoroutine(ScrollBackground());
        }
    }
    public void StopBackground()
    {
        if (backgroundCoroutine != null)
        {
            StopCoroutine(backgroundCoroutine);
            backgroundCoroutine = null;

            ResetBackgroundPositions();
        }
    }
    private void ResetBackgroundPositions()
    {
        if (backgroundImages == null)
            return;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                backgroundImages[i, j].transform.position
                    = new Vector3(spriteWidth * i, spriteWidth * j, 0f);
            }
        }

        //foreach (Transform background in backgrounds)
        //{
        //    if (initialPositions.ContainsKey(background))
        //    {
        //        background.position = initialPositions[background];
        //    }
        //}
    }
    IEnumerator ScrollBackground()
    {
        while (true)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var rightEdgeOfImage = backgroundImages[i, j].transform.position.x + spriteWidth;

                    if (rightEdgeOfImage <= 0)
                    {
                        if (i == 0)
                        {
                            backgroundImages[i, j].transform.position
                   = new Vector3(backgroundImages[x - 1, j].transform.position.x + spriteWidth, backgroundImages[x - 1, j].transform.position.y);
                            backgroundImages[i, j].transform.Translate(new Vector3(-scrollSpeed * Time.deltaTime, 0, 0));
                        }
                        else
                        {
                            backgroundImages[i, j].transform.position
                   = new Vector3(backgroundImages[i - 1, j].transform.position.x + spriteWidth, backgroundImages[i - 1, j].transform.position.y);
                        }

                    }
                    else
                    {
                        backgroundImages[i, j].transform.Translate(new Vector3(-scrollSpeed * Time.deltaTime, 0, 0));
                    }
                }
            }
            yield return null;
        }
    }

    private int currentBackgroundIndex = 0;
    private void SetImages()
    {
        if (backgroundSprites == null || backgroundSprites.Count == 0)
        {
            Debug.LogError("Background sprites are not loaded.");
            return;
        }

        icon.GetComponent<Image>().sprite = backgroundSprites[currentBackgroundIndex];
        ToolManager.Instance.BackgroundName = backgroundSprites[currentBackgroundIndex].name;

        backGroundName = ToolManager.Instance.BackgroundName;
        backGroundName1 = backGroundName + "1";

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                backgroundImages[i, j].name = "BackgroundTile_" + i + "_" + j;

                Sprite sprite;

                if (j == 0)
                    sprite = Resources.Load<Sprite>("Sprite/Backgrounds/" + backGroundName);
                else
                    sprite = Resources.Load<Sprite>("Sprite/Backgrounds/" + backGroundName1);

                backgroundImages[i, j].GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
    }

    public void BackgroundImageUp()
    {
        currentBackgroundIndex++;
        if (currentBackgroundIndex >= backgroundSprites.Count)
        {
            currentBackgroundIndex = 0; // 리스트의 처음으로 롤오버합니다.
        }
        SetImages();
    }

    public void BackgroundImageDown()
    {
        currentBackgroundIndex--;
        if (currentBackgroundIndex < 0)
        {
            currentBackgroundIndex = backgroundSprites.Count - 1; // 리스트의 마지막으로 롤오버합니다.
        }
        SetImages();
    }
}
