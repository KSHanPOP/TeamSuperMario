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
        set { typeName = value; }
    }
    public String IconSpritePath
    {
        get { return iconSpritePath; }
    }

    public void OnPopup()
    {
        
    }
    public void OnMouseDown()
    {
        //ClickChangeTile.prefapInfo = this.TypeName;
    }
}
