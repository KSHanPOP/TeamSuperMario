using UnityEngine;
using System.Collections.Generic;

public class IngameManagerTest : MonoBehaviour
{
    private static IngameManagerTest instance;
    public static IngameManagerTest Instance { get { return instance; } }

    private LinkedList<DynamicTile> dynamicTiles = new ();

    public LinkedList<DynamicTile> DynamicTiles { get { return dynamicTiles; } }

    private void Awake()
    {
        instance = this;
    }    
    public void StartTest()
    {
        foreach(var dynamicTile in dynamicTiles)
        {
            dynamicTile.StartTest();
        }
    }
    public void StopTest()
    {
        foreach (var dynamicTile in dynamicTiles)
        {
            dynamicTile.StopTest();
        }
    }
}
