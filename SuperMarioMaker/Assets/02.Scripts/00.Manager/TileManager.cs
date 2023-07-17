using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{   
    public static TileManager Instance { get; private set; }

    private Transform dynamicObjHolder;
    public Transform DynamicObjHolder { get { return dynamicObjHolder; } }

    public bool IsPlaying { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }
    public void StartTest()
    {
        if (IsPlaying)
            return;
        
        IsPlaying = true;
        MovementLimmiter.instance.CharacterCanMove = true;
        
        dynamicObjHolder = new GameObject("DynamicObjHolder").transform;
        BaseTile.StartTest();
        PipeWarpConnector.StartTest();

        PopupManager.Instance.OffPopups();
    }
    public void StopTest()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;
        
        Destroy(dynamicObjHolder.gameObject);
        BaseTile.StopTest();        
        PipeWarpConnector.StopTest();
    }

    public void Restart()
    {
        StopTest();
        StartTest();
    }
}
