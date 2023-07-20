using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompPopupGetter : PopupGetter
{
    [SerializeField]
    private DynamicTile_Thwomp thwompTile;

    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(2).GetComponent<ThwompPopup>();

        popup.Enter(thwompTile.Dir);

        SetPosition(popup.transform);

        popup.EventPopupOff.AddListener(OffPopup);
        SetToggleListener(popup);

        base.OnPopup();
    }

    private void SetToggleListener(ThwompPopup popup)
    {        
        for(int i = 0; i < 3; i++)
        {
            int index = i;
            popup.GetToggle(index).onValueChanged
            .AddListener(value => { if (value) thwompTile.Dir = index; });            
        }        
    }
}
