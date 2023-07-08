using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompPopupGetter : PopupGetter
{
    [SerializeField]
    private Thwomp thwomp;

    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(2).GetComponent<ThwompPopup>();

        popup.Enter(thwomp.GetDir());

        popup.EventPopupOff.AddListener(OffPopup);
        SetToggleListener(popup);

        base.OnPopup();
    }

    private void SetToggleListener(ThwompPopup popup)
    {
        for(int i = 0; i < 3; i++)
        {
            popup.GetToggle(i).onValueChanged
            .AddListener(value => { if (value) thwomp.SetDir(i); });
        }        
    }
}
