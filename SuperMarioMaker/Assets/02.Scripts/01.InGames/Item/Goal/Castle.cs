using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer rightSprite;
    public static Castle Instance { get; private set; }    

    private bool isClear = false;    

    private void Awake()
    {
        Instance = this;
    }

    public void Clear()
    {
        isClear = true;
        rightSprite.sortingOrder = (int)EnumSpriteLayerOrder.Player + 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (!isClear)
            return;

        Logger.Debug("game clear!");
        DynamicTileManager.Instance.StopTest();
    }
}
