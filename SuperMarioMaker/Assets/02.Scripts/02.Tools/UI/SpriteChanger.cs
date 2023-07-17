using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteChanger : MonoBehaviour
{
    public Sprite newSprite; // 이 스프라이트를 인스펙터에서 설정하거나 코드로 로드할 수 있습니다.

    void Start()
    {
        Image imageComponent = GetComponent<Image>(); // 이 게임 오브젝트에 있는 Image 컴포넌트를 가져옵니다.
        imageComponent.sprite = newSprite; // sprite 속성을 새 스프라이트로 설정합니다.
    }
}
