using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public bool UseCustomPoint = false;

    [SerializeField]
    private float customHeightValue;

    [SerializeField]
    private float customStartValue;

    [SerializeField]
    private float customEndValue;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private Camera cam;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        cam = Camera.main;

        if (UseCustomPoint)
        {
            minX = customStartValue;
            maxX = customEndValue;
            maxY = customHeightValue;
            minY = customHeightValue;

            Update();
            cam.GetComponent<SleepMonsterAwaker>().enabled = true;

            return;
        }

        GameData gameData;

        if (SceneLoader.Instance.State == SceneState.Tool)
        {
            gameData = GameManager.instance.gameData;
        }
        else
        {
            gameData = InGameManager.Instance.GameData;
        }        

        minX = 24 / 2;
        maxX = 24 * gameData.MapRowLength - minX;
        minY = 13.5f / 2;
        maxY = 13.5f * gameData.MapRowLength - minY;

        Update();
        cam.GetComponent<SleepMonsterAwaker>().enabled = true;
    }

    private void Update()
    {
        Vector3 targetPosition = transform.TransformPoint(new Vector3(0, 0, -10));
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
        smoothPosition.z = -10;
        cam.transform.position = smoothPosition;
    }
}
