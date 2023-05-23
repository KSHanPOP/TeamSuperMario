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
        startPos = transform.position; // ó�� ��ġ ����
    }

    void Update()
    {
        newPosition += speed * Time.deltaTime; // �� ��ġ ���
        transform.position = startPos + Vector3.left * newPosition; // �� ��ġ ����

        if (newPosition > backgroundSize) // ����� ȭ�� ������ ������
        {
            startPos += Vector3.right * backgroundSize; // ó�� ��ġ�� ��� ũ�⸸ŭ ���������� �̵�
            newPosition -= backgroundSize; // �� ��ġ�� ��� ũ�⸸ŭ ����
        }
    }
}
