using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ETileType
{
    None, Default, Selected, unSelected,
}
public class PrefapInfo : MonoBehaviour
{
    // [SerializeField] String iconPath;

    [SerializeField] private String iconSpritePath;
    [SerializeField] private ETileType typeName;
    public ETileType TypeName
    {
        get { return typeName; }
    }
    public String IconSpritePath
    {
        get { return iconSpritePath; }
    }

    public void OnMouseDown()
    {
        if (DynamicTileManager.Instance.IsPlaying)
            return;

        Logger.Debug(transform.position);
    }
}
