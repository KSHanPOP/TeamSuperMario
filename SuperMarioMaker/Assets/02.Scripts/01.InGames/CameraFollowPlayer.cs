using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
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
    }

    void Update()
    {
        var gameData = GameManager.instance.gameData;

        minX = gameData.TileX / 2;
        maxX = gameData.TileX * gameData.MapRowLength - minX;
        minY = gameData.TileY / 2;
        maxY = gameData.TileY * gameData.MapRowLength - minY;

        Vector3 targetPosition = transform.TransformPoint(new Vector3(0, 0, -10));
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
        smoothPosition.z = -10;
        cam.transform.position = smoothPosition;
    }
}
