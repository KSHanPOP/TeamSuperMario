using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TilePopup : MonoBehaviour
{
    public UnityEvent EventPopupOff;

    public int Index { get; set; }

    public virtual void ClearListeners() { }

    protected virtual void OnEnable()
    {
        TogglePopup();
    }

    protected virtual void TogglePopup()
    {
        var popups = PopupManager.Instance.GetPopups();
        
        for(int i = 0;  i < popups.Length; i++)
        {
            if (i == Index)
                continue;

            popups[i].SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        EventPopupOff.Invoke();
        ClearListeners();
    }
}
