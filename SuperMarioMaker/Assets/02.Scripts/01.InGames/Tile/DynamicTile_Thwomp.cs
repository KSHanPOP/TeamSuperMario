using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile_Thwomp : DynamicTile
{
    [SerializeField]
    private PrefapInfo prefapInfo;

    //private void Awake()
    //{
    //    var collider = GetComponent<BoxCollider2D>();
    //    collider.enabled = false;

    //    var layerMask = LayerMask.GetMask("Platform", "Monster", "Player", "Default", "MonsterNoCollision", "Coin", "Default");
    //    var hit = Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.5f), Vector2.one * 1.5f, 0f, Vector2.zero, layerMask);
    //    if (hit)
    //    {   
    //        Destroy(gameObject);
    //    }

    //    collider.enabled = true;
    //}

    private int dir;
    public int Dir 
    {
        get { return dir; } 
        set 
        { 
            dir = value;
            prefapInfo.TileValue1 = value;
        } 
    }

    public void SetValue(int info1, int info2 = 0)
    {
        Dir = info1;
    }

    public override void Play()
    {
        var thwomp = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder).GetComponent<Thwomp>();
        thwomp.SetDir(dir);
        gameObject.SetActive(false);
    }
}
