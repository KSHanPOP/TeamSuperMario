using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float height;

    [SerializeField]
    Camera cam;

    private float leftClamp;
    private float rightClamp;

    private Transform playerTransform;

    private bool isPlaying;

    Vector3 newPos = Vector3.zero;

    private float cameraHeight;
    private float cameraWidth;
    private void Awake()
    {
        cameraHeight = cam.orthographicSize * 2f;
        cameraWidth = cameraHeight * cam.aspect;
    }
    public void Init(float startPoint, float endPoint)
    {
        leftClamp = startPoint + cameraWidth * 0.5f;
        rightClamp = endPoint - cameraWidth * 0.5f;
    }
    public void SetPlayer(Transform player)
    {
        playerTransform = player;
        isPlaying = true;
    }

    public void End()
    {
        isPlaying = false;
    }

    public void Update()
    {
        if (!isPlaying)
            return;

        newPos.x = Mathf.Clamp(playerTransform.position.x, leftClamp, rightClamp);
    }
}
