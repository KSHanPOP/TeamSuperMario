using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSize;
    public float speed;

    private Vector3 startPos;
    private float newPosition;

    void Start()
    {
        startPos = transform.position; // 처음 위치 저장
    }

    void Update()
    {
        newPosition += speed * Time.deltaTime; // 새 위치 계산
        transform.position = startPos + Vector3.left * newPosition; // 새 위치 설정

        if (newPosition > backgroundSize) // 배경이 화면 밖으로 나가면
        {
            startPos += Vector3.right * backgroundSize; // 처음 위치를 배경 크기만큼 오른쪽으로 이동
            newPosition -= backgroundSize; // 새 위치를 배경 크기만큼 줄임
        }
    }
}
