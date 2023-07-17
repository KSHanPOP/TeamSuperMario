using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteChanger : MonoBehaviour
{
    public Sprite newSprite; // �� ��������Ʈ�� �ν����Ϳ��� �����ϰų� �ڵ�� �ε��� �� �ֽ��ϴ�.

    void Start()
    {
        Image imageComponent = GetComponent<Image>(); // �� ���� ������Ʈ�� �ִ� Image ������Ʈ�� �����ɴϴ�.
        imageComponent.sprite = newSprite; // sprite �Ӽ��� �� ��������Ʈ�� �����մϴ�.
    }
}
