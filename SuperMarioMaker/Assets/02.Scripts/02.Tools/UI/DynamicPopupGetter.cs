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

        dynamicTilePopup.Enter(dynamicTileBlock.GetItemCount());

        SetPosition(popup.transform);

        SetButtonListeners();
        SetSliderListeners();
    }
    private void SetPosition(Transform popupTransform)
    {
        var newPos = Camera.main.WorldToScreenPoint(transform.position);    

        newPos.x = Mathf.Clamp(newPos.x, clampX, Screen.width - clampX);
        newPos.y = Mathf.Clamp(newPos.y, clampY, Screen.height - clampY);

        popupTransform.position = newPos;
    }

    private void SetButtonListeners()
    {
        dynamicTilePopup.GetButton(EnumItems.Coin).onClick
            .AddListener(()=> dynamicTileBlock.SetItemType(EnumItems.Coin));

        dynamicTilePopup.GetButton(EnumItems.FireFlower).onClick
            .AddListener(() => dynamicTileBlock.SetItemType(EnumItems.FireFlower));

        dynamicTilePopup.GetButton(EnumItems.Mushroom).onClick
            .AddListener(() => dynamicTileBlock.SetItemType(EnumItems.Mushroom));
    }
    private void SetSliderListeners()
    {
        dynamicTilePopup.GetSlider().onValueChanged
            .AddListener(value => dynamicTileBlock.SetItemCount(value));
    }
}
