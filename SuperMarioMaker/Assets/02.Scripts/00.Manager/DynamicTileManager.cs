using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTileManager : MonoBehaviour
{   
    public static DynamicTileManager Instance { get; private set; }

    private LinkedList<DynamicTile> dynamicTiles = new();
    public LinkedList<DynamicTile> DynamicTiles { get { return dynamicTiles; } }

    private LinkedList<DynamicTile> buffer = new();

    private Transform dynamicObjHolder;
    public Transform DynamicObjHolder { get { return dynamicObjHolder; } }

    private bool isPlaying = false;
    private void Awake()
    {
        Instance = this;
    }
    public void StartTest()
    {
        if (isPlaying)
            return;

        MovementLimmiter.instance.CharacterCanMove = true;

        isPlaying = true;

        dynamicObjHolder = new GameObject("DynamicObjHolder").transform;

        foreach (DynamicTile tile in dynamicTiles)
        {
            if (tile == null)
                continue;            

            tile.StartTest();

            buffer.AddLast(tile);
        }

        SwapBuffer();

    }
    public void StopTest()
    {
        if (!isPlaying)
            return;

        isPlaying = false;

        Destroy(dynamicObjHolder.gameObject);

        foreach (DynamicTile tile in dynamicTiles)
        {
            tile.StopTest();
        }
    }

    public void SwapBuffer()
    {
        dynamicTiles.Clear();

        (buffer, dynamicTiles) = (dynamicTiles, buffer);
    }

    public void Restart()
    {
        StopTest();
        StartTest();
    }
}
