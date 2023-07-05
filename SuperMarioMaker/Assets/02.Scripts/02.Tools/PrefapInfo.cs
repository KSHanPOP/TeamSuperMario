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
    private void Awake()
    {   
        IsDynamic = TryGetComponent<DynamicTile>(out dynamicTile);
    }
    public void OnMouseDown()
    {
        //ClickChangeTile.prefapInfo = this.TypeName;        
    }
    public void OnMouseUp()
    {
        OnPopup();
    }

    public void EnableCommand()
    {
        if (TryGetComponent<ICommandStackAble>(out ICommandStackAble stackAble))
            stackAble.EnableCommand();
    }
    public void DisableCommand()
    {
        if (TryGetComponent<ICommandStackAble>(out ICommandStackAble stackAble))
            stackAble.DisableCommand();
    }
    public void OnPopup()
    {
        if (TryGetComponent<PopupGetter>(out PopupGetter popupGetter))
            popupGetter.OnPopup();
    }
}
