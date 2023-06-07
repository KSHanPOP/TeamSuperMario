using System.Collections.Generic;
using UnityEngine;

public class IngameObjectManager : MonoBehaviour
{
    private static IngameObjectManager instance;
    public static IngameObjectManager Instance
    {
        get
        {
            if(instance == null)
            {   
                GameObject obj = new(typeof(IngameObjectManager).Name);
                instance = obj.AddComponent<IngameObjectManager>();
            }
            return instance;
        }
    }

    public int Capacity { get; set; } = 0;

    private List<DynamicTile> dynamicTiles = new List<DynamicTile>();
    public List<DynamicTile> DynamicTiles { get { return dynamicTiles; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Logger.Debug("StartInstance");
            Logger.Debug(dynamicTiles.Count);
            InstatnceAll();
        }
    }

    public void InstatnceAll()
    {
        foreach(DynamicTile tile in dynamicTiles)
        {
            tile.InstancePrefab();
        }            
    }
}
