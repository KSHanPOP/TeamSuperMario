using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    [SerializeField]
    protected GameObject dynamicObject;

    protected DynamicTileManager dynamicTileManager;

    protected virtual void Start()
    {
        dynamicTileManager = DynamicTileManager.Instance;
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
}
