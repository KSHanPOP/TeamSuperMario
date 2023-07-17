using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : BaseTile
{    
    [SerializeField]
    protected GameObject dynamicObject;

    protected TileManager dynamicTileManager;

    public GameObject Instantiated { get; set; }

    private void Start()
    {
        dynamicTileManager = TileManager.Instance;        
    }

    public override void Play()
    {
        Instantiated = Instantiate(dynamicObject, transform.position, Quaternion.identity, dynamicTileManager.DynamicObjHolder);

        gameObject.SetActive(false);
    }

    public override void Stop()
    {
        gameObject.SetActive(true);
    }
}
