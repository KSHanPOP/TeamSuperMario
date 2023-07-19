using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
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

        if(GameManager.instance == null)
        {
            minX = customStartValue;
            maxX = customEndValue;
            maxY = customHeightValue;
            minY = customHeightValue;
            return;
        }   

        var gameData = GameManager.instance.gameData;

        minX = gameData.TileX / 2;
        maxX = gameData.TileX * gameData.MapRowLength - minX;
        minY = gameData.TileY / 2;
        maxY = gameData.TileY * gameData.MapRowLength - minY;
    }

    void Update()
    {
        Vector3 targetPosition = transform.TransformPoint(new Vector3(0, 0, -10));
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
        smoothPosition.z = -10;
        cam.transform.position = smoothPosition;
    }
}
