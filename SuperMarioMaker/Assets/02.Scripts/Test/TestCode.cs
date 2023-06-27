using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public void Event()
    {
        Logger.Debug("Clicked");
    }

    public void OnMouseDown()
    {
        Logger.Debug(name);
    }
}
