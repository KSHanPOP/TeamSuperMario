using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ThwompPopup : TilePopup
{
    [SerializeField]
    private Toggle[] toggles;
    public Toggle GetToggle(int idx) => toggles[idx];

    public void Enter(int idx)
    {
        EventPopupOff.Invoke();
        gameObject.SetActive(true);
        ClearListeners();

        toggles[idx].isOn = true;
    }

    public override void ClearListeners()
    {
        foreach(var toggle in toggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }

        EventPopupOff.RemoveAllListeners();
    }
}
