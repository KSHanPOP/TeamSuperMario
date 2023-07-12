using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile_Thwomp : DynamicTile
{
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

    public int Dir { get; set; }

    public override void StartTest()
    {
        var thwomp = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder).GetComponent<Thwomp>();
        thwomp.SetDir(Dir);
        gameObject.SetActive(false);
    }
}
