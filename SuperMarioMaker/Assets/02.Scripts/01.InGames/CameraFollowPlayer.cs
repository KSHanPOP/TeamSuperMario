using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Update()
    {
        if (player != null)
        {
            // Set the boundaries
            minX = 0f + ToolManager.Instance.TilemapX / 2;
            maxX = ToolManager.Instance.TilemapX * ToolManager.Instance.MapRowLength - minX;
            minY = 0f + ToolManager.Instance.TilemapY / 2;
            maxY = ToolManager.Instance.TilemapY * ToolManager.Instance.MapColLength - minY;

            Vector3 targetPosition = player.TransformPoint(new Vector3(0, 0, -10));
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            smoothPosition.x = Mathf.Clamp(smoothPosition.x, minX, maxX);
            smoothPosition.y = Mathf.Clamp(smoothPosition.y, minY, maxY);
            transform.position = smoothPosition;
        }
    }
}
