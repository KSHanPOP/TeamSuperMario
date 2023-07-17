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

        for(int i = 0; i < popups.Length; i++)
        {
            popups[i].GetComponent<TilePopup>().Index = i;
        }
    }        

    public void OffPopups()
    {
        foreach (var popup in popups)
        {
            popup.SetActive(false);
        }
    }

    public GameObject GetPopup(int index) => popups[index];
    public GameObject[] GetPopups() => popups;
}
