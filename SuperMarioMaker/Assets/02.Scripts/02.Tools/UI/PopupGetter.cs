using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupGetter : MonoBehaviour
{
    [SerializeField]
    protected float clampX;
    [SerializeField] 
    protected float clampY;
    public virtual void OnPopup()
    {

    }
        
}
