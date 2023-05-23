using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int numberOfPlatforms;
    public float recycleOffset;
    public Vector3 platformStartPoint;
    public Vector3 platformSpacing;

    private Vector3 nextPosition;
    private Queue<GameObject> platformQueue;

    void Start()
    {
        platformQueue = new Queue<GameObject>(numberOfPlatforms);
        nextPosition = platformStartPoint;

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            GameObject platform = (GameObject)Instantiate(platformPrefab);
            platform.transform.position = nextPosition;
            nextPosition += platformSpacing;
            platformQueue.Enqueue(platform);
        }
    }

    void Update()
    {
        if (platformQueue.Peek().transform.position.x + recycleOffset < Camera.main.transform.position.x)
        {
            GameObject platform = platformQueue.Dequeue();
            platform.transform.position = nextPosition;
            nextPosition += platformSpacing;
            platformQueue.Enqueue(platform);
        }
    }
}