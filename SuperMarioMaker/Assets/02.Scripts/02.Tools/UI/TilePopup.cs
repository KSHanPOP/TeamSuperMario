using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TilePopup : MonoBehaviour
{
    public UnityEvent EventPopupOff;

    public virtual void ClearListeners() { }

    protected virtual void OnDisable()
    {
        EventPopupOff.Invoke();
        ClearListeners();
    }
}
