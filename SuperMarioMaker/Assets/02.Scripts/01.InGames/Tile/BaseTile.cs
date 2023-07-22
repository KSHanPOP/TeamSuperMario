using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    public static LinkedList<BaseTile> Tiles = new();
    public static LinkedList<BaseTile> Buffer = new();

    protected bool IsListed = false;

    protected virtual void OnEnable()
    {
        if (IsListed)
            return;

        Tiles.AddLast(this);
        IsListed = true;
    }

    public static void StartGame()
    {
        foreach (var tile in Tiles)
        {
            if (tile == null)
                continue;

            if (!tile.gameObject.activeSelf)
            {
                tile.Pop();
                continue;
            }

            tile.Play();
            Buffer.AddLast(tile);
        }

        SwapBuffer();
    }
    public static void StopGame()
    {
        foreach (var tile in Tiles)
        {
            tile.Stop();
        }
    }
    protected static void SwapBuffer()
    {
        Tiles.Clear(); 
        (Tiles, Buffer) = (Buffer, Tiles);
    }

    protected void Pop()
    {
        IsListed = false;
    }

    public virtual void Play() { }
    public virtual void Stop() { }

}
