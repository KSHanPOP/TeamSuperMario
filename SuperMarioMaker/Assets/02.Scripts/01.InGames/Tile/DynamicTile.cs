using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    [SerializeField]
    protected GameObject dynamicObject;

    protected GameObject objectHolder;

    protected IngameManagerTest testManager;

    protected virtual void Start()
    {
        testManager = IngameManagerTest.Instance;
        testManager.DynamicTiles.AddLast(this);
    }

    public virtual void StartTest()
    {
        objectHolder = Instantiate(dynamicObject, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    public virtual void StopTest()
    {
        if(objectHolder != null)
            Destroy(objectHolder);

        gameObject.SetActive(true);
    }
}
