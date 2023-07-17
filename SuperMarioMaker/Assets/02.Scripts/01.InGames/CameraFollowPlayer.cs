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
        var gamedata = GameManager.instance.gameData;

        minX = gamedata.TileX / 2;
        maxX = gamedata.TileX * gamedata.MapRowLength - minX;
        minY = gamedata.TileY / 2;
        maxY = gamedata.TileY * gamedata.MapRowLength - minY;

        Vector3 targetPosition = transform.TransformPoint(new Vector3(0, 0, -10));
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
        cam.transform.position = smoothPosition;
    }
}
