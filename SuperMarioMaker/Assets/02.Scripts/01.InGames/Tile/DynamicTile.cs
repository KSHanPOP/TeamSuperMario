using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    [SerializeField]
    protected GameObject dynamicObject;

    protected TileManager dynamicTileManager;

    protected virtual void Start()
    {
        dynamicTileManager = TileManager.Instance;
        dynamicTileManager.DynamicTiles.AddLast(this);        
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
    public void AddToTilesList()
    {
        dynamicTileManager.DynamicTiles.AddLast(this);
    }
    public void RemoveFromTilesList()
    {
        dynamicTileManager.DynamicTiles.Remove(this);
    }
}
