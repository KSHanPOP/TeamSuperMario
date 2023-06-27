using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefapInfo : MonoBehaviour
{
    // [SerializeField] String iconPath;

    [SerializeField] private String iconSpritePath;
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
