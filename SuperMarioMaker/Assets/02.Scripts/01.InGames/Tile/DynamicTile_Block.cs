using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile_Block : DynamicTile
{
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    protected EnumItems itemType;

    [SerializeField]
    protected int itemCount;

    [SerializeField]
    protected bool isTransparent;

    protected Block block;

    protected override void Start()
    {
        base.Start();        
    }

    private void SetBlockValue()
    {
        block = objectHolder.GetComponent<Block>();
        block.ItemType = itemType;
        block.ItemCount = itemCount;
    }

    public override void StartTest()
    {
        objectHolder = Instantiate(dynamicObject, transform.position, Quaternion.identity);
        SetBlockValue();
        block.InitSetting();
        gameObject.SetActive(false);
    }

    public Sprite[] GetSprites()
    {
        return sprites;
    }
    public void SetItemType(int index)
    {
        itemType = (EnumItems)index;
    }
    public void SetCoinCount(int count)
    {
        itemCount = count;
    }

}
