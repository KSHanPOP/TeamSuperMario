using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    private PrefapInfo prefapInfo;

    [SerializeField]
    private EnumPipeEntrancePos pipeEntrancePos;

    public EnumPipeEntrancePos PipeEntrancePos { get { return pipeEntrancePos; } }

    [SerializeField]
    private int maxLength = 10;

    private int minLength = 2;

    [SerializeField]
    private GameObject[] verticalEntrance;

    [SerializeField]
    private GameObject[] verticalPillar;

    [SerializeField]
    private GameObject[] horizontalEntrance;

    [SerializeField]
    private GameObject[] horizontalPillar;

    [SerializeField]
    private BoxCollider2D boxCollider2D;

    [SerializeField]
    private GameObject highlight;

    [SerializeField]
    private GameObject plant;

    public bool IsUpward { get; set; }

    [SerializeField]
    private bool banPlant;
    public bool BanPlant
    {
        get { return banPlant; }
        set
        {
            prefapInfo.TileInfo1 = value ? 1 : 0;
            banPlant = value;
            plant.SetActive(!banPlant);
        }
    }

    private LayerMask layerMask;

    private bool isVertical;

    private Vector2 direction;

    private int length;
    public int Length { get { return length; } }

    public void SetValue(int info1, int info2)
    {
        BanPlant = info1 == 1;
        Setlength(info2);
    }

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster", "Player", "Default", "MonsterNoCollision", "Coin");
        IsUpward = pipeEntrancePos == EnumPipeEntrancePos.Top;
        isVertical = (int)pipeEntrancePos % 2 == 0;
        direction = GetDirection();
    }

    private void Start()
    {
        MakeEntrance();
        MakePillar();
        SetPillarLength(minLength);
        SetCollider();
        SetHighlight();
    }

    private Vector2 GetDirection() => pipeEntrancePos switch
    {
        EnumPipeEntrancePos.Top => Vector2.down,
        EnumPipeEntrancePos.Left => Vector2.right,
        EnumPipeEntrancePos.Bottom => Vector2.up,
        EnumPipeEntrancePos.Right => Vector2.left,
        _ => Vector2.zero,
    };

    private void MakeEntrance()
    {
        if (IsUpward)
            plant = Instantiate(plant, transform.position + Vector3.up, Quaternion.identity);

        //if (BoxCast())
        //    Destroy(gameObject);

        if (isVertical)
        {
            var leftSprite = Instantiate(verticalEntrance[0], transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            var rightSprite = Instantiate(verticalEntrance[1], transform.position + Vector3.right, Quaternion.identity, transform).GetComponent<SpriteRenderer>();

            leftSprite.flipY = pipeEntrancePos == EnumPipeEntrancePos.Bottom;
            rightSprite.flipY = pipeEntrancePos == EnumPipeEntrancePos.Bottom;
        }
        else
        {
            var upSprite = Instantiate(horizontalEntrance[0], transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            var downSprite = Instantiate(horizontalEntrance[1], transform.position + Vector3.down, Quaternion.identity, transform).GetComponent<SpriteRenderer>();

            upSprite.flipX = pipeEntrancePos == EnumPipeEntrancePos.Right;
            downSprite.flipX = pipeEntrancePos == EnumPipeEntrancePos.Right;
        }
    }
    private bool BoxCast()
    {
        Vector2 pos = (Vector2)transform.position;
        Vector2 startPos = pipeEntrancePos switch
        {
            EnumPipeEntrancePos.Top => pos + new Vector2(0.5f, -0.5f),
            EnumPipeEntrancePos.Left => pos + new Vector2(0.5f, -0.5f),
            EnumPipeEntrancePos.Bottom => pos + new Vector2(0.5f, 0.5f),
            EnumPipeEntrancePos.Right => pos + new Vector2(-0.5f, -0.5f),
            _ => Vector2.zero,
        };

        boxCollider2D.enabled = false;

        var hit = Physics2D.BoxCast(startPos, Vector2.one * 1.5f, 0f, Vector2.zero, layerMask);

        boxCollider2D.enabled = true;

        return hit;
    }
    public int GetMaxLength()
    {
        boxCollider2D.enabled = false;
        int maxValue = 10;

        var hit1 = Physics2D.Raycast(transform.position, direction, maxValue, layerMask);

        var hit2 = Physics2D.Raycast((Vector2)transform.position + (isVertical ? Vector2.right : Vector2.down), direction, maxValue, layerMask);

        boxCollider2D.enabled = true;

        int distance1 = hit1 ? Mathf.CeilToInt(hit1.distance) : maxValue;

        int distance2 = hit2 ? Mathf.CeilToInt(hit2.distance) : maxValue;

        return distance1 < distance2 ? distance1 : distance2;
    }

    private void MakePillar()
    {
        for (int i = 1; i < maxLength; i++)
        {
            Vector3 pillarPos1 = transform.position + (Vector3)direction * i;

            Vector3 pillarPos2 = pillarPos1 + (isVertical ? Vector3.right : Vector3.down);

            Instantiate(isVertical ? verticalPillar[0] : horizontalPillar[0], pillarPos1, Quaternion.identity, transform);

            Instantiate(isVertical ? verticalPillar[1] : horizontalPillar[1], pillarPos2, Quaternion.identity, transform);
        }
    }
    public void SetPillarLength(int value)
    {
        length = value;

        int activeCount = 2 + length * 2;

        int childCount = transform.childCount;

        for (int i = 0; i < activeCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = activeCount; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void SetCollider()
    {
        boxCollider2D.size = isVertical ? new Vector2(2f, length) : new Vector2(length, 2f);

        bool isPositive = pipeEntrancePos == EnumPipeEntrancePos.Left || pipeEntrancePos == EnumPipeEntrancePos.Bottom;

        float offset = (length - 1) * 0.5f * (isPositive ? 1 : -1);

        boxCollider2D.offset = isVertical ? new Vector2(0.5f, offset) : new Vector2(offset, -0.5f);
    }
    private void SetHighlight()
    {
        highlight.transform.localScale = boxCollider2D.size;
        highlight.transform.localPosition = boxCollider2D.offset;
    }
    public void Setlength(float value)
    {
        prefapInfo.TileInfo2 = (int)value;
        SetPillarLength((int)value);
        SetCollider();
        SetHighlight();
    }

    public void SleepPlant()
    {
        if (banPlant)
            return;

        if (!IsUpward)
            return;

        if (plant.GetComponent<DynamicTile>().Instantiated == null)
            return;

        plant.GetComponent<DynamicTile>().Instantiated
            .GetComponent<PlantMove>().SleepCollision();        
    }

    private void OnEnable()
    {
        if (!banPlant && plant != null)
            plant.SetActive(true);
    }

    private void OnDisable()
    {
        if (!banPlant && plant != null)
            plant.SetActive(false);
    }
}

public enum EnumPipeEntrancePos
{
    None = -1,
    Top,
    Left,
    Bottom,
    Right,
}