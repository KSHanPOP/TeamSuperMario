using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarOnOffButton : MonoBehaviour
{
    public Animator buttonAnimator;
    private bool isOnOff = false;

    public void OnButtonClick()
    {
        if (!isOnOff)
        {
            buttonAnimator.SetTrigger("Off");
            //buttonAnimator.Play("YourAnimationName");
            isOnOff = true;
        }
        else
        {
            buttonAnimator.SetTrigger("On");
            //buttonAnimator.Play("YourAnimationName");
            isOnOff = false;
        }

    }
}
