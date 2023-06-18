using System;
using System.Collections;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField]
    private bool isTransparent;

    [SerializeField]
    private Sprite usedSprite;

    [SerializeField]
    private EnumItems itemType;

    [SerializeField]
    private int coinCount;

    [SerializeField]
    private float shakeTime;

    [SerializeField]
    private float maxShakeHeight;

    private bool isHitable = true;
    
    private SpriteRenderer spriteRenderer;

    private Transform spriteTransform;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteTransform = spriteRenderer.transform;
        SetStartSprite();
    }
    private void SetStartSprite()
    {
        if (isTransparent)
            spriteRenderer.sprite = null;
    }
    public void SmallHit()
    {
        if (!isHitable)
            return;
        
        StartCoroutine(ShakeCoroutine());
    }
    public void BigHit()
    {
        if (!isHitable)
            return;

        Logger.Debug("bigHIt");
    }

    IEnumerator ShakeCoroutine()
    {
        isHitable = false;

        float timer = 0f;
        float height;
        float divShakeTime = 1 / shakeTime;        

        Vector3 newPos = Vector3.zero;

        while(timer < shakeTime)
        {
            timer += Time.deltaTime;                       
            height = Mathf.Lerp(0f, maxShakeHeight, Mathf.Sin(timer * divShakeTime * Mathf.PI));
            
            newPos.y = height;

            spriteTransform.localPosition = newPos;           

            yield return null;
        }

        spriteTransform.localPosition = Vector3.zero;
        isHitable = true;
        yield break;
    }
}
