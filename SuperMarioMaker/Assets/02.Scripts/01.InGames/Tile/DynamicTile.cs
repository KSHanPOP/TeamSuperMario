using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    [SerializeField]
    GameObject dynamicObject;

    private GameObject objectHolder;

    IngameManagerTest testManager;

    private void Start()
    {
        testManager = IngameManagerTest.Instance;
        testManager.DynamicTiles.AddLast(this);
    }

    public void StartTest()
    {
        objectHolder = Instantiate(dynamicObject, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    public void StopTest()
    {
        if(objectHolder != null)
            Destroy(objectHolder);

        gameObject.SetActive(true);
    }
}
