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
    public Tilemap levelTilemap; // 레벨의 Tilemap
    public GameObject backgroundPrefab; // 배경 이미지의 프리팹
    public Transform backgroundParent; // 배경 이미지들의 부모 객체

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

        // Resources/Background 폴더에서 모든 스프라이트를 로드합니다.
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
                // 배경 이미지의 오른쪽 경계를 계산합니다.
                float backgroundRightBoundary = background.position.x + background.localScale.x / 2;
                float backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
                // 배경 이미지의 오른쪽 경계가 타일맵의 왼쪽 경계를 넘어갔다면,
                if (backgroundRightBoundary <= -backgroundWidth)
                {
                    float maxBackgroundPosX = Mathf.Max(Array.ConvertAll(backgrounds, b => b.position.x));
                    background.position = new Vector2(maxBackgroundPosX + backgroundWidth, background.position.y);
                }

                // 배경 이미지를 왼쪽으로 이동시킵니다.
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

        // 각 배경 오브젝트의 스프라이트를 변경합니다.
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
            currentBackgroundIndex = 0; // 리스트의 처음으로 롤오버합니다.
        }
        ChangeBackgroundImage();
    }

    public void BackgroundImageDown()
    {
        currentBackgroundIndex--;
        if (currentBackgroundIndex < 0)
        {
            currentBackgroundIndex = backgroundSprites.Count - 1; // 리스트의 마지막으로 롤오버합니다.
        }
        ChangeBackgroundImage();
    }
}
