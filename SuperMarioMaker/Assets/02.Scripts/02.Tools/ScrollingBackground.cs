using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSpeed = 0.1f;
    public Tilemap levelTilemap; // 레벨의 Tilemap
    public GameObject backgroundPrefab; // 배경 이미지의 프리팹
    public Transform backgroundParent; // 배경 이미지들의 부모 객체

    private Transform[] backgrounds;
    private Coroutine backgroundCoroutine;
    private Dictionary<Transform, Vector2> initialPositions = new Dictionary<Transform, Vector2>();

    private void Init()
    {
        foreach (Transform background in backgrounds)
        {
            initialPositions[background] = background.position;
        }
    }
    void InitializeBackgrounds()
    {
        float worldWidth = levelTilemap.cellBounds.max.x - levelTilemap.cellBounds.min.x;
        float backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        int backgroundCount = Mathf.CeilToInt(worldWidth / backgroundWidth) + 1;

        backgrounds = new Transform[backgroundCount];

        for (int i = 0; i < backgroundCount; i++)
        {
            GameObject newBackground = Instantiate(backgroundPrefab, backgroundParent);
            newBackground.transform.position = new Vector2(levelTilemap.cellBounds.min.x + i * backgroundWidth, 0);
            backgrounds[i] = newBackground.transform;
        }
        Init();
    }
    void Start()
    {
        InitializeBackgrounds();
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
                background.position = new Vector2(background.position.x - backgroundSpeed, background.position.y);
                if (background.position.x <= -background.localScale.x)
                {
                    background.position = new Vector2(background.position.x + background.localScale.x * 2, background.position.y);
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}