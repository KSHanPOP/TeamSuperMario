using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{   
    public static TileManager Instance { get; private set; }

    private LinkedList<DynamicTile> dynamicTiles = new();
    public LinkedList<DynamicTile> DynamicTiles { get { return dynamicTiles; } }

    private LinkedList<DynamicTile> buffer = new();

    private Transform dynamicObjHolder;
    public Transform DynamicObjHolder { get { return dynamicObjHolder; } }

    public bool IsPlaying { get; private set; } = false;
    private void Awake()
    {
        Instance = this;
    }
    public void StartTest()
    {
        if (IsPlaying)
            return;

        MovementLimmiter.instance.CharacterCanMove = true;

        IsPlaying = true;

        OnDynamics();

        OnStatics();

        SwapBuffer();        
    }
    public void StopTest()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;

        OffDynamics();

        OffStatics();
    }

    public void SwapBuffer()
    {
        dynamicTiles.Clear();

        (buffer, dynamicTiles) = (dynamicTiles, buffer);
    }

    private void OnDynamics()
    {
        dynamicObjHolder = new GameObject("DynamicObjHolder").transform;

        foreach (DynamicTile tile in dynamicTiles)
        {
            if (tile == null)
                continue;

            if (!tile.gameObject.activeSelf)
            {
                tile.IsPoped = true;
                continue;
            }

            tile.StartTest();

            buffer.AddLast(tile);
        }
    }
    private void OffDynamics()
    {
        Destroy(dynamicObjHolder.gameObject);

        foreach (DynamicTile tile in dynamicTiles)
        {
            tile.StopTest();
        }
    }
    private void OnStatics()
    {
        PipeWarpConnector.StartTest();
        StaticTile.StartTest();
    }

    private void OffStatics()
    {
        PipeWarpConnector.StopTest();
        StaticTile.StopTest();
    }

    public void Restart()
    {
        StopTest();
        StartTest();
    }
}
