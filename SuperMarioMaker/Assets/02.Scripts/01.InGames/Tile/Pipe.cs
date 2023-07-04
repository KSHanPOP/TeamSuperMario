using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
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

    private LayerMask layerMask;

    private bool isVertical;

    private Vector2 direction;

    private int length;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster", "Player", "Default");
        isVertical = (int)pipeEntrancePos % 2 == 0;
        direction = GetDirection();
    }

    private void Start()
    {
        MakeEntrance();
        //GetLength();
        MakePillar();
        //SetCollider();
        SetPillarLength(0);
    }

    private Vector2 GetDirection() => pipeEntrancePos switch
    {
        EnumPipeEntrancePos.Top => Vector2.down,
        EnumPipeEntrancePos.Left => Vector2.right,
        EnumPipeEntrancePos.Bottom => Vector2.up,
        EnumPipeEntrancePos.Right => Vector2.left,
        _=> Vector2.zero,
    };

    private void MakeEntrance()
    {
        if (isVertical)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, 1f, layerMask))
                Destroy(gameObject);

            var leftSprite = Instantiate(verticalEntrance[0], transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            var rightSprite = Instantiate(verticalEntrance[1], transform.position + Vector3.right, Quaternion.identity, transform).GetComponent<SpriteRenderer>();

            leftSprite.flipY = pipeEntrancePos == EnumPipeEntrancePos.Bottom;
            rightSprite.flipY = pipeEntrancePos == EnumPipeEntrancePos.Bottom;
        }
        else
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, 1f, layerMask))
                Destroy(gameObject);

            var upSprite = Instantiate(horizontalEntrance[0], transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            var downSprite = Instantiate(horizontalEntrance[1], transform.position + Vector3.down, Quaternion.identity, transform).GetComponent<SpriteRenderer>();

            upSprite.flipX = pipeEntrancePos == EnumPipeEntrancePos.Right;
            downSprite.flipX = pipeEntrancePos == EnumPipeEntrancePos.Right;
        }
    }

    private void GetLength()
    {
        var hit1 = Physics2D.Raycast(transform.position, direction, maxLength + 1, layerMask);

        var hit2 = Physics2D.Raycast((Vector2)transform.position + (isVertical ? Vector2.right : Vector2.down), direction, maxLength + 1, layerMask);

        int distance1 = hit1 ? Mathf.CeilToInt(hit1.distance) : maxLength;

        int distance2 = hit2 ? Mathf.CeilToInt(hit2.distance) : maxLength;

        length = distance1 < distance2 ? distance1 : distance2;
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
    public void SetPillarLength(int length)
    {
        if (length < minLength)
            length = minLength;

        int activeCount = length * 2 + 2;

        int childCount = transform.childCount;

        for(int i = 0; i < activeCount; i++)
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

        boxCollider2D.offset = isVertical ? new Vector2(0.5f, offset) : new Vector2(offset, - 0.5f);
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