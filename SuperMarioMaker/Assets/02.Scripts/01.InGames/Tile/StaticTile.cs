using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTile : MonoBehaviour
{
    public static LinkedList<StaticTile> StaticTiles = new();
    public static LinkedList<StaticTile> Buffer = new();

    public bool IsInList;

    private void Awake()
    {
        IsInList = false;        
    }

    protected virtual void OnEnable()
    {
        if (IsInList)
            return;

        StaticTiles.AddLast(this);
        IsInList = true;
    }

    public static void StartTest()
    {
        foreach (var staticTile in StaticTiles)
        {
            if (!staticTile.gameObject.activeSelf)
            {
                staticTile.Pop();
                continue;
            }

            if (staticTile != null)
            {
                staticTile.Play();
                Buffer.AddLast(staticTile);
            }
        }
    }
    public static void StopTest()
    {
        SwapBuffer();

        foreach (var staticTile in StaticTiles)
        {
            staticTile.Stop();
        }
    }
    protected static void SwapBuffer()
    {
        StaticTiles.Clear();

        (StaticTiles, Buffer) = (Buffer, StaticTiles);
    }

    protected void Pop()
    {
        IsInList = false;
    }

    protected virtual void Play() { }
    protected virtual void Stop() { }


}
