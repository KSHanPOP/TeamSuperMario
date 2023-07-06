using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPopupGetter : PopupGetter
{
    [SerializeField]
    private bool isQusetion;

    [SerializeField]
    private DynamicTile_Block dynamicTileBlock;

    private void Start()
    {
        float horizontalValue = Mathf.Lerp(1, -1, transform.position.x / 20);
        float verticalValue = Mathf.Lerp(1, -1, transform.position.y / (180/16));

        posMover = Vector3.right * horizontalValue + Vector3.up * verticalValue;          
    }

    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(0).GetComponent<DynamicTilePopup>();

        popup.GetSlider().minValue = isQusetion ? 1 : 0;

        popup.Enter(dynamicTileBlock.GetItemType() ,dynamicTileBlock.GetItemCount());

        SetPosition(popup.transform);
        
        SetToggleListener(popup);
        SetSliderListener(popup);
    }
    public override void OffPopup()
    {
        spriteRenderer.color = Color.white;
    }
    private void SetToggleListener(DynamicTilePopup popup)
    {
        popup.GetToggle(EnumItems.Coin).onValueChanged
            .AddListener(value => { if(value) dynamicTileBlock.SetItemType(EnumItems.Coin); });

        popup.GetToggle(EnumItems.Mushroom).onValueChanged
            .AddListener(value => { if (value) dynamicTileBlock.SetItemType(EnumItems.Mushroom); });

        popup.GetToggle(EnumItems.FireFlower).onValueChanged
            .AddListener(value => { if (value) dynamicTileBlock.SetItemType(EnumItems.FireFlower); });
    }

    private void SetSliderListener(DynamicTilePopup popup)
    {
        popup.GetSlider().onValueChanged
            .AddListener(value => dynamicTileBlock.SetItemCount(value));
    }
}
