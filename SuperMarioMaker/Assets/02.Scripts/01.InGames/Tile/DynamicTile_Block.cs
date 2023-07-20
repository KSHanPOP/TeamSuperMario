using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile_Block : DynamicTile
{
    [SerializeField]
    protected PrefapInfo prefapInfo;

    [SerializeField]
    protected EnumItems itemType;

    [SerializeField]
    protected int itemCount;

    [SerializeField]
    protected bool isTransparent;

    protected Block block;

    public void SetValue(int info1, int info2)
    {
        SetItemType((EnumItems)info1);
        SetItemCount((int)info2);
    }

    private void SetBlockValue()
    {   
        block.ItemType = itemType;
        block.ItemCount = itemCount;
    }

    public override void Play()
    {
        block = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder).GetComponent<Block>();
        SetBlockValue();
        block.InitSetting();
        gameObject.SetActive(false);
    }
    public Sprite[] GetSprites() => ItemSpawnManagers.Instance.Sprites;
    public void SetItemType(EnumItems items)
    {
        prefapInfo.TileInfo1 = (int)items;
        itemType = items;
    }
    public EnumItems GetItemType() => itemType;
    public void SetItemCount(float count)
    {
        prefapInfo.TileInfo2 = (int)count;
        itemCount = (int)count;
    }
    public int GetItemCount() => itemCount;

}
