using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    [SerializeField]
    protected GameObject dynamicObject;

    protected TileManager dynamicTileManager;

    public bool IsPoped = false;

    protected virtual void Start()
    {
        dynamicTileManager = TileManager.Instance;
        dynamicTileManager.DynamicTiles.AddLast(this);        
    }

    protected virtual void OnEnable()
    {
        if (!IsPoped)
            return;

        dynamicTileManager.DynamicTiles.AddLast(this);
        IsPoped = false;
    }

    public virtual void StartTest()
    {
        Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder);

        gameObject.SetActive(false);
    }

    public virtual void StopTest()
    {
        gameObject.SetActive(true);
    }
    //public void DisableCommand()
    //{
    //    dynamicTileManager.DynamicTiles.Remove(this);
    //}

    //public void EnableCommand()
    //{
    //    dynamicTileManager.DynamicTiles.AddLast(this);
    //}
}
