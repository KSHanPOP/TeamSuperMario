using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarOnOffButton : MonoBehaviour
{
    public Animator buttonAnimator;
    private bool isOnOff = false;
    public bool IsOnOff { get { return isOnOff; } }

    public void OnButtonClick()
    {
        if (!isOnOff)
        {
            buttonAnimator.SetTrigger("Off");
            isOnOff = true;
        }
        else
        {
            buttonAnimator.SetTrigger("On");
            isOnOff = false;
        }

    }
}
