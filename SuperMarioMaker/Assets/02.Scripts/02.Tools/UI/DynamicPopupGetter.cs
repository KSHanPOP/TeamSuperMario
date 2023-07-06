using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicPopupGetter : PopupGetter
{
    private DynamicTilePopup dynamicTilePopup;

    [SerializeField]
    private bool isQusetion;

    [SerializeField]
    private DynamicTile_Block dynamicTileBlock;
    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(0);
        dynamicTilePopup = popup.GetComponent<DynamicTilePopup>();

        dynamicTilePopup.GetSlider().minValue = isQusetion ? 1 : 0;

        dynamicTilePopup.Enter(dynamicTileBlock.GetItemType() ,dynamicTileBlock.GetItemCount());

        SetPosition(popup.transform);
        
        SetToggleListeners();
        SetSliderListeners();
    }

    private void SetPosition(Transform popupTransform)
    {
        var newPos = Camera.main.WorldToScreenPoint(transform.position);

        newPos.x = Mathf.Clamp(newPos.x, clampX, Screen.width - clampX);
        newPos.y = Mathf.Clamp(newPos.y, clampY, Screen.height - clampY);

        popupTransform.position = newPos;
    }
    private void SetToggleListeners()
    {
        //Toggle[] toggles = dynamicTilePopup.GetToggles();

        //for (int i = 0; i < toggles.Length; i++)
        //{
        //    toggles[i].onValueChanged
        //        .AddListener(value => { if (value == true) SetItemType((EnumItems)i); });
        //}

        dynamicTilePopup.GetToggle(EnumItems.Coin).onValueChanged
            .AddListener(value => { if(value) SetItemType(EnumItems.Coin); });

        dynamicTilePopup.GetToggle(EnumItems.Mushroom).onValueChanged
            .AddListener(value => { if (value) SetItemType(EnumItems.Mushroom); });

        dynamicTilePopup.GetToggle(EnumItems.FireFlower).onValueChanged
            .AddListener(value => { if (value) SetItemType(EnumItems.FireFlower); });
    }
    private void SetItemType(EnumItems type)
    {   
        dynamicTileBlock.SetItemType(type);
    }

    private void SetSliderListeners()
    {
        dynamicTilePopup.GetSlider().onValueChanged
            .AddListener(value => dynamicTileBlock.SetItemCount(value));
    }
}
