using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector2 currentMove = Vector2.zero;

    float minX;
    float maxX;
    float minY;
    float maxY;
    void Update()
    {
        if (currentMove != Vector2.zero && ToolManager.Instance.ToolMode == ToolManager.ToolModeType.Tool)
        {

            // Move the camera
            Camera.main.transform.position += new Vector3(currentMove.x, currentMove.y, 0) * speed * Time.deltaTime;

            // Set the boundaries
            minX = 0f + ToolManager.Instance.TilemapX / 2;
            maxX = ToolManager.Instance.TilemapX * ToolManager.Instance.MapRowLength - minX;
            minY = 0f + ToolManager.Instance.TilemapY / 2;
            maxY = ToolManager.Instance.TilemapY * ToolManager.Instance.MapColLength - minY;

            // Get the current camera position
            Vector3 pos = Camera.main.transform.position;

            // Clamp the camera position
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            // Update the camera position
            Camera.main.transform.position = pos;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        currentMove = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            currentMove = context.ReadValue<Vector2>();
        }

    }
}
