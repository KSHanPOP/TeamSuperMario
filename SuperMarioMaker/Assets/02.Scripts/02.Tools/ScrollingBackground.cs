using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] public float backgroundSpeed;
    public Tilemap levelTilemap; // ������ Tilemap
    public GameObject backgroundPrefab; // ��� �̹����� ������
    public Transform backgroundParent; // ��� �̹������� �θ� ��ü

    private Transform[] backgrounds;
    private Transform[,] backgrounds2D;

    private Coroutine backgroundCoroutine;
    private Dictionary<Transform, Vector2> initialPositions = new Dictionary<Transform, Vector2>();

    [SerializeField]
    private Button upButton;
    [SerializeField]
    private Button downButton;

    private void Init()
    {
        //foreach (Transform background in backgrounds)
        //{
        //    initialPositions[background] = background.position;
        //}

        for (int i = 0; i < backgrounds2D.GetLength(0); i++)
        {
            for (int j = 0; j < backgrounds2D.GetLength(1); j++)
            {
                initialPositions[backgrounds2D[i, j]] = backgrounds2D[i, j].position;
            }
        }

        upButton.onClick.AddListener(BackgroundImageUp);
        downButton.onClick.AddListener(BackgroundImageDown);
    }
    void InitializeBackgrounds()
    {
        //float worldWidth = ToolManager.Instance.TilemapX - 1;
        //float hightWidth = ToolManager.Instance.TilemapY - 1;
        //float backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        //float backgroundHight = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        //int backgroundCount = Mathf.CeilToInt(worldWidth / backgroundWidth) + 2;

        //backgrounds = new Transform[backgroundCount];

        //for (int i = 0; i < backgroundCount; i++)
        //{
        //    GameObject newBackground = Instantiate(backgroundPrefab, backgroundParent);
        //    newBackground.transform.position = new Vector2(i * backgroundWidth, 0);
        //    backgrounds[i] = newBackground.transform;
        //}

        float worldWidth = ToolManager.Instance.TilemapX* ToolManager.Instance.MapRowLength - 1;
        float worldHeight = ToolManager.Instance.TilemapY * ToolManager.Instance.MapColLength - 1;
        float backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float backgroundHeight = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        int backgroundCountX = Mathf.CeilToInt(worldWidth / backgroundWidth) + 2;
        int backgroundCountY = Mathf.CeilToInt(worldHeight / backgroundHeight);

        backgrounds2D = new Transform[backgroundCountX, backgroundCountY];

        for (int i = 0; i < backgroundCountX; i++)
        {
            for (int j = 0; j < backgroundCountY; j++)
            {
                GameObject newBackground = Instantiate(backgroundPrefab, backgroundParent);
                newBackground.transform.position = new Vector2(i * backgroundWidth, j * backgroundHeight);
                backgrounds2D[i, j] = newBackground.transform;
            }
        }

    }

    private List<Sprite> backgroundSprites;
    private void LoadBackgroundSprites()
    {
        backgroundSprites = new List<Sprite>();

        // Resources/Background �������� ��� ��������Ʈ�� �ε��մϴ�.
        object[] loadedSprites = Resources.LoadAll("Background", typeof(Sprite));

        foreach (Sprite sprite in loadedSprites)
        {
            backgroundSprites.Add(sprite);
        }
    }

    void Start()
    {
        Logger.CheckNullObject(this);

        InitializeBackgrounds();
        Init();
        LoadBackgroundSprites();
        //MoveBackground();
        ChangeBackgroundImage();
    }

    public void SetSizeChange()
    {
        for (int i = 0; i < backgrounds2D.GetLength(0); i++)
        {
            for (int j = 0; j < backgrounds2D.GetLength(1); j++)
            {
                if (backgrounds2D[i, j] != null)
                {
                    // ����Ƽ ��ü �ı�
                    Destroy(backgrounds2D[i, j].gameObject);
                }
            }
        }

        InitializeBackgrounds();
        Init();
        ChangeBackgroundImage();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    BackgroundImageUp();
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
        foreach (Transform background in backgrounds)
        {
            if (initialPositions.ContainsKey(background))
            {
                background.position = initialPositions[background];
            }
        }
    }
    IEnumerator ScrollBackground()
    {
        while (true)
        {


            float maxBackgroundPosX = float.MinValue;

            for (int a = 0; a < backgrounds2D.GetLength(0); a++)
            {
                for (int b = 0; b < backgrounds2D.GetLength(1); b++)
                {
                    var bg = backgrounds2D[a, b];
                    if (bg != null && bg.position.x > maxBackgroundPosX)
                    {
                        maxBackgroundPosX = bg.position.x;
                    }
                }
            }

            //background.transform.position = new Vector2(maxBackgroundPosX + backgroundWidth, background.transform.position.y);
            yield return null;
        }

        //while (true)
        //{
        //    for (int i = 0; i < backgrounds2D.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < backgrounds2D.GetLength(1); j++)
        //        {
        //            var background = backgrounds2D[i, j];
        //            // ��� �̹����� ������ ��踦 ����մϴ�.
        //            float backgroundRightBoundary = background.transform.position.x + background.transform.localScale.x / 2;
        //            float backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;

        //            // ��� �̹����� ������ ��谡 Ÿ�ϸ��� ���� ��踦 �Ѿ�ٸ�,
        //            if (backgroundRightBoundary <= -backgroundWidth)
        //            {
        //                float maxBackgroundPosX = Mathf.Max(Array.ConvertAll(backgrounds, b => b.position.x));
        //                background.transform.position = new Vector2(maxBackgroundPosX + backgroundWidth, background.transform.position.y);
        //            }

        //            // ��� �̹����� �������� �̵���ŵ�ϴ�.
        //            background.transform.position = new Vector2(background.transform.position.x - backgroundSpeed, background.transform.position.y);
        //        }
        //    }
        //    yield return null;
        //}

        //while (true)
        //{
        //    foreach (Transform background in backgrounds)
        //    {
        //        // ��� �̹����� ������ ��踦 ����մϴ�.
        //        float backgroundRightBoundary = background.position.x + background.localScale.x / 2;
        //        float backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        //        // ��� �̹����� ������ ��谡 Ÿ�ϸ��� ���� ��踦 �Ѿ�ٸ�,
        //        if (backgroundRightBoundary <= -backgroundWidth)
        //        {
        //            float maxBackgroundPosX = Mathf.Max(Array.ConvertAll(backgrounds, b => b.position.x));
        //            background.position = new Vector2(maxBackgroundPosX + backgroundWidth, background.position.y);
        //        }

        //        // ��� �̹����� �������� �̵���ŵ�ϴ�.
        //        background.position = new Vector2(background.position.x - backgroundSpeed, background.position.y);
        //    }
        //    yield return null;
        //}
    }

    private int currentBackgroundIndex = 0;
    private void ChangeBackgroundImage()
    {
        if (backgroundSprites == null || backgroundSprites.Count == 0)
        {
            Debug.LogError("Background sprites are not loaded.");
            return;
        }

        // �� ��� ������Ʈ�� ��������Ʈ�� �����մϴ�.
        //foreach (Transform background in backgrounds)
        //{
        //    background.GetComponent<SpriteRenderer>().sprite = backgroundSprites[currentBackgroundIndex];
        //}
        for (int i = 0; i < backgrounds2D.GetLength(0); i++)
        {
            for (int j = 0; j < backgrounds2D.GetLength(1); j++)
            {
                backgrounds2D[i, j].GetComponent<SpriteRenderer>().sprite = backgroundSprites[currentBackgroundIndex];
            }
        }


        icon.GetComponent<Image>().sprite = backgroundSprites[currentBackgroundIndex];
        ToolManager.Instance.BackgroundName = backgroundSprites[currentBackgroundIndex].name;
    }

    public void BackgroundImageUp()
    {
        currentBackgroundIndex++;
        if (currentBackgroundIndex >= backgroundSprites.Count)
        {
            currentBackgroundIndex = 0; // ����Ʈ�� ó������ �ѿ����մϴ�.
        }
        ChangeBackgroundImage();
    }

    public void BackgroundImageDown()
    {
        currentBackgroundIndex--;
        if (currentBackgroundIndex < 0)
        {
            currentBackgroundIndex = backgroundSprites.Count - 1; // ����Ʈ�� ���������� �ѿ����մϴ�.
        }
        ChangeBackgroundImage();
    }
}
