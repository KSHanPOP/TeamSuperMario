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
    [SerializeField]
    public bool IsDynamic;
    // [SerializeField] String iconPath;

    [SerializeField] private String iconSpritePath;
    [SerializeField] private ETileType typeName;

    private DynamicTile dynamicTile;
    public ETileType TypeName
    {
        get { return typeName; }
        set { typeName = value; }
    }
    public String IconSpritePath
    {
        get { return iconSpritePath; }
    }

    public void OnMouseDown()
    {
        //ClickChangeTile.prefapInfo = this.TypeName;                
    }

    public void OnMouseUpAsButton()
    {
        OnPopup();
    }
    public void OnPopup()
    {
        if (PipeWarpConnector.IsLinking)
            return;

        if (TryGetComponent<PopupGetter>(out PopupGetter popupGetter))
            popupGetter.OnPopup();
    }
}
