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
        Vector3 cameraPosition = Camera.main.transform.position;

        float horizontalValue = (transform.position.x - cameraPosition.x) < 0 ? 1f : -1f;
        float verticalValue = (transform.position.y - cameraPosition.y) < 0 ? 1f : -1f;

        posMover = new Vector3(horizontalValue * 1.3f, verticalValue, 0);
    }
    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(0).GetComponent<DynamicTilePopup>();

        popup.Enter(dynamicTileBlock.GetItemType(), dynamicTileBlock.GetItemCount(), isQusetion);

        SetPosition(popup.transform);

        popup.EventPopupOff.AddListener(OffPopup);
        SetToggleListener(popup);
        SetSliderListener(popup);

        base.OnPopup();
    }

    private void SetToggleListener(DynamicTilePopup popup)
    {
        popup.GetToggle(EnumItems.Coin).onValueChanged
            .AddListener(value => { if (value) dynamicTileBlock.SetItemType(EnumItems.Coin); });

        popup.GetToggle(EnumItems.Mushroom).onValueChanged
            .AddListener(value => { if (value) dynamicTileBlock.SetItemType(EnumItems.Mushroom); });

        popup.GetToggle(EnumItems.FireFlower).onValueChanged
            .AddListener(value => { if (value) dynamicTileBlock.SetItemType(EnumItems.FireFlower); });
    }

    private void SetSliderListener(DynamicTilePopup popup)
    {
        popup.GetSlider().onValueChanged
            .AddListener(dynamicTileBlock.SetItemCount);
    }
}
