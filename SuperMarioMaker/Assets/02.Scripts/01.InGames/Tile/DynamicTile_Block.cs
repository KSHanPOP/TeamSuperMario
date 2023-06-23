using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile_Block : DynamicTile
{
    [SerializeField]
    protected EnumItems itemType;

    [SerializeField]
    protected int itemCount;

    [SerializeField]
    protected bool isTransparent;

    protected Block block;

    private void SetBlockValue()
    {   
        block.ItemType = itemType;
        block.ItemCount = itemCount;
    }

    public override void StartTest()
    {
        block = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder).GetComponent<Block>();
        SetBlockValue();
        block.InitSetting();
        gameObject.SetActive(false);
    }

    public Sprite[] GetSprites()
    {
        return ItemSpawnManagers.Instance.Sprites;
    }
    public void SetItemType(int index)
    {
        itemType = (EnumItems)index;
    }
    public void SetItemCount(int count)
    {
        itemCount = count;
    }
}
