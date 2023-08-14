using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour
{
    private float mapRowLength;
    private float mapColLength;
    private string backGroundName;
    private string backGroundName1;

    public float scrollSpeed;
    private int x;
    private int y;

    [SerializeField] private GameObject tempObject;
    GameObject[,] backgroundImages;
    float spriteWidth;

    private IEnumerator moveCoroutine;
    private void Awake()
    {

    }

    void Start()
    {
        Init();
        CalculateNumberOfBackgroundImages();
        SetImages();
        SetPos();
    }

    private void Init()
    {
        mapRowLength = InGameManager.Instance.GameData.MapRowLength * 24;
        mapColLength = InGameManager.Instance.GameData.MapColLength * 13.5f;

        backGroundName = InGameManager.Instance.GameData.BackGround;
        backGroundName1 = InGameManager.Instance.GameData.BackGround + "1";
    }

    private void CalculateNumberOfBackgroundImages()
    {
        spriteWidth = Resources.Load<Sprite>("Sprite/Backgrounds/Background Ground").bounds.size.x;

        x = Mathf.CeilToInt(mapRowLength / spriteWidth) + 1;
        y = Mathf.CeilToInt(mapColLength / spriteWidth);

    }

    private void SetImages()
    {
        backgroundImages = new GameObject[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                backgroundImages[i, j] = Instantiate(tempObject);
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

    public void SetPos()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                backgroundImages[i, j].transform.position
                    = new Vector3(spriteWidth * i, spriteWidth * j, 0f);
            }
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    float rightEdgeOfImage = backgroundImages[i, j].transform.position.x + spriteWidth;

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
                   = new Vector3(backgroundImages[i - 1, j].transform.position.x + spriteWidth, backgroundImages[x - 1, j].transform.position.y);
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

    public void StartMove()
    {
        moveCoroutine = Move();
        StartCoroutine(moveCoroutine);
    }

    public void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        SetPos();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
