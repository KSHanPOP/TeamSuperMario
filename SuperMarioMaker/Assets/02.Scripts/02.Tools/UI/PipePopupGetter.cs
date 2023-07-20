using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipePopupGetter : PopupGetter
{
    [SerializeField]
    private Pipe pipe;

    [SerializeField]
    private PipeWarpConnector connector;

    protected override void Start()
    {
        posMoverMult = 1.5f;
        base.Start();
    }

    public override void OnPopup()
    {
        var popup = PopupManager.Instance.GetPopup(1).GetComponent<PipePopup>();

        popup.Enter(pipe.BanPlant, pipe.GetMaxLength(), pipe.Length);

        SetPosition(popup.transform);

        popup.EventPopupOff.AddListener(connector.ClearHighlight);
        SetToggleListener(popup);
        SetButtonListener(popup);
        SetSliderListener(popup);

        connector.DrawHighlight();
        base.OnPopup();
    }

    public void SetToggleListener(PipePopup popup)
    {
        if(!pipe.IsUpward)
        {
            popup.OffToggleInteract();
            return;
        }

        var toggle = popup.GetToggle();

        toggle.interactable = true;

        toggle.onValueChanged.AddListener(value => pipe.BanPlant = value);

        SetPosition(popup.transform);
    }
    public void SetButtonListener(PipePopup popup)
    {
        popup.GetButton().onClick
            .AddListener(connector.StartLink);
    }
    public void SetSliderListener(PipePopup popup)
    {
        popup.GetSlider().onValueChanged
            .AddListener(pipe.Setlength);
    }    
}
