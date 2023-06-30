using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    private bool isVertical;

    [SerializeField]
    private int maxLength = 10;

    [SerializeField]
    private Sprite[] verticalEntrance;

    [SerializeField]
    private GameObject[] verticalPillar;

    [SerializeField]
    private Sprite[] horizontalEntrance;

    [SerializeField]
    private GameObject[] horizontalPillar;

    [SerializeField]
    private BoxCollider2D boxCollider2D;

    private LayerMask layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Platform", "Monster", "Player");
    }

    private void Start()
    {
        Logger.Debug(GetLength());

        MakePillar(GetLength());        
    }

    private int GetLength()
    {
        var hit1 = Physics2D.Raycast(transform.position, isVertical ? Vector2.down : Vector2.right, maxLength + 1, layerMask);

        var hit2 = Physics2D.Raycast((Vector2)transform.position + (isVertical ? Vector2.right : Vector2.down), isVertical ? Vector2.down : Vector2.right, maxLength + 1, layerMask);

        int distance1 = hit1 ? Mathf.FloorToInt(hit1.distance) : maxLength;

        int distance2 = hit2 ? Mathf.FloorToInt(hit2.distance) : maxLength;

        return distance1 < distance2 ? distance1 : distance2;
    }
    private void MakePillar(int length)
    {
        for(int i = 0; i < length; i++)
        {
            Vector3 pillarPos1 = transform.position + (isVertical ? Vector3.down * (i + 1) : Vector3.right * (i + 1));

            Vector3 pillarPos2 = pillarPos1 + (isVertical ? Vector3.right : Vector3.down);

            Instantiate(isVertical ? verticalPillar[0] : horizontalPillar[0], pillarPos1, Quaternion.identity,transform);

            Instantiate(isVertical ? verticalPillar[1] : horizontalPillar[1], pillarPos2, Quaternion.identity, transform);
        }
    }
}
