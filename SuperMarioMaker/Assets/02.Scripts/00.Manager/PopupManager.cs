using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject[] popups; 
    private void Awake()
    {
        Instance = this;        
    }        
    public GameObject GetPopup(int index) => popups[index];     
}
