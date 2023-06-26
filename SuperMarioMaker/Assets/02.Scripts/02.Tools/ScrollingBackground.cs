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
    [SerializeField] public float backgroundSpeed;
    public Tilemap levelTilemap; // ������ Tilemap
    public GameObject backgroundPrefab; // ��� �̹����� ������
    public Transform backgroundParent; // ��� �̹������� �θ� ��ü

    private Transform[] backgrounds;
    private Coroutine backgroundCoroutine;
    private Dictionary<Transform, Vector2> initialPositions = new Dictionary<Transform, Vector2>();

    [SerializeField]
    private Button upButton;
    [SerializeField]
    private Button downButton;

    private void Init()
    {
        foreach (Transform background in backgrounds)
        {
            initialPositions[background] = background.position;
        }

        upButton.onClick.AddListener(BackgroundImageUp);
        downButton.onClick.AddListener(BackgroundImageDown);
    }
    void InitializeBackgrounds()
    {
        //float worldWidth = levelTilemap.cellBounds.max.x - levelTilemap.cellBounds.min.x;
        float worldWidth = ToolManager.Instance.TilemapX - 1;
        float backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        int backgroundCount = Mathf.CeilToInt(worldWidth / backgroundWidth) + 2;

        backgrounds = new Transform[backgroundCount];

        for (int i = 0; i < backgroundCount; i++)
        {
            GameObject newBackground = Instantiate(backgroundPrefab, backgroundParent);
            newBackground.transform.position = new Vector2(i * backgroundWidth, 0);
            backgrounds[i] = newBackground.transform;
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
            foreach (Transform background in backgrounds)
            {
                // ��� �̹����� ������ ��踦 ����մϴ�.
                float backgroundRightBoundary = background.position.x + background.localScale.x / 2;
                float backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
                // ��� �̹����� ������ ��谡 Ÿ�ϸ��� ���� ��踦 �Ѿ�ٸ�,
                if (backgroundRightBoundary <= -backgroundWidth)
                {
                    float maxBackgroundPosX = Mathf.Max(Array.ConvertAll(backgrounds, b => b.position.x));
                    background.position = new Vector2(maxBackgroundPosX + backgroundWidth, background.position.y);
                }

                // ��� �̹����� �������� �̵���ŵ�ϴ�.
                background.position = new Vector2(background.position.x - backgroundSpeed, background.position.y);
            }
            yield return null;
        }
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
        foreach (Transform background in backgrounds)
        {
            background.GetComponent<SpriteRenderer>().sprite = backgroundSprites[currentBackgroundIndex];
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
