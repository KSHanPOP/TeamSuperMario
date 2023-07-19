using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button deleteButton;
    void Start()
    {
        startButton.onClick.AddListener(StartOn);
        deleteButton.onClick.AddListener(DeleteOn);
    }

    public void StartOn()
    {
        SaveLoadManager.Instance.LoadGame(gameObject.name);
    }
    public void DeleteOn()
    {
        SaveLoadManager.Instance.Delete(gameObject.name);
    }

}
