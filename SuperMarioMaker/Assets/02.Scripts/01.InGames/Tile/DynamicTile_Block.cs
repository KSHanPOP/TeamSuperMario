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

    public override void Play()
    {
        block = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder).GetComponent<Block>();
        SetBlockValue();
        block.InitSetting();
        gameObject.SetActive(false);
    }
    public Sprite[] GetSprites() => ItemSpawnManagers.Instance.Sprites;
    public void SetItemType(EnumItems items) => itemType = items;
    public EnumItems GetItemType() => itemType;
    public void SetItemCount(float count) => itemCount = (int)count;
    public int GetItemCount() => itemCount;

}
