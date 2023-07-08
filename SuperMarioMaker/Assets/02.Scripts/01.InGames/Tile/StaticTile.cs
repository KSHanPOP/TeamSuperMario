using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTile : MonoBehaviour
{
    public static LinkedList<StaticTile> StaticTiles = new();
    public static LinkedList<StaticTile> Buffer = new();

    public bool IsPoped = false;

    private void Awake()
    {
        StaticTiles.AddLast(this);
    }

    protected virtual void OnEnable()
    {
        if (!IsPoped)
            return;

        StaticTiles.AddLast(this);
        IsPoped = false;
    }

    public static void StartTest()
    {
        foreach (var staticTile in StaticTiles)
        {
            if (!staticTile.gameObject.activeSelf)
            {
                staticTile.IsPoped = true;
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

    protected virtual void Play() { }
    protected virtual void Stop() { }


}
