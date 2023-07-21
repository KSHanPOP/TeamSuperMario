using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTracker : MonoBehaviour
{
    [SerializeField]
    private TileManager tileManager;

    [SerializeField]
    private KeyCode enterKey;

    [SerializeField]
    private KeyCode exitKey;

    [SerializeField]
    private KeyCode startTrackingKey;  

    [SerializeField]
    private SpeedGraphDrawer speedGraphDrawer;

    private bool isStarted = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(enterKey))
        {
            if(!isStarted)
            {
                SoundManager.Instance.PlayBGM("SMB2GROUND");
            }

            isStarted = true;
            
            tileManager.StartTest();
        }

        if(Input.GetKeyDown(exitKey))
        {
            speedGraphDrawer.StopTrackSpeed();

            tileManager.StopTest();
        }

        if(Input.GetKeyDown(startTrackingKey))
        {
            if (!tileManager.IsPlaying)
                return;

            speedGraphDrawer.StartTrackSpeed();
        }
    }
}
