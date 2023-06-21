using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField]
    private bool isTransparent;

    [SerializeField]
    protected Sprite spriteAfterUseItem;

    [SerializeField]
    protected EnumItems itemType;

    [SerializeField]
    protected int coinCount;

    protected int itemCount;

    [SerializeField]
    protected float shakeTime;

    [SerializeField]
    protected float maxShakeHeight;

    protected bool isHitable = true;

    private BoxCollider2D detectorWhenShaking;

    protected SpriteRenderer spriteRenderer;

    protected Transform spriteTransform;

    protected ItemSpawnManagers spawnManagers;

    private HashSet<IShakeable> shakenObjects = new HashSet<IShakeable>();

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOder.Block;
        spriteTransform = spriteRenderer.transform;
        detectorWhenShaking = GetComponent<BoxCollider2D>();

        itemCount = itemType == EnumItems.Coin ? (coinCount > 0 ? coinCount : 1) : 1;

        SetStartSprite();
    }
    protected virtual void Start()
    {
        spawnManagers = ItemSpawnManagers.Instance;        
    }
    protected virtual void SetStartSprite()
    {
        if (isTransparent)
            spriteRenderer.sprite = null;
    }
    public virtual void NormalHit()
    {
        if (!isHitable)
            return;
        
        StartCoroutine(ShakeCoroutine());
    }
    public virtual void BigHit()
    {
        if (!isHitable)
            return;

        if(itemType == EnumItems.None)
        {
            Logger.Debug("crash");
            return;
        }

        NormalHit();
    }

    protected virtual void StartInstanceItem(Vector2 startPos, Vector2 destPos)
    {
        if (itemType == EnumItems.None)
            return;        

        var item = Instantiate(spawnManagers.prefabs[(int)itemType], startPos, Quaternion.identity);
        item.GetComponent<ItemBase>().StartInstance(destPos);
    }

    protected virtual void CheckItemRemainCount()
    {
        if (itemType == EnumItems.None)
            return;

        --itemCount;

        if (itemCount == 0)
            spriteRenderer.sprite = spriteAfterUseItem;
    }
    protected virtual void CheckHitable()
    {
        if (itemType == EnumItems.None)
        {
            isHitable = true;
            return;
        }

        isHitable = itemCount > 0;
    }

    protected virtual IEnumerator ShakeCoroutine()
    {
        isHitable = false;
        detectorWhenShaking.enabled = true;

        float timer = 0f;
        float inverseShakeTime = 1 / shakeTime;        
        float normalizedTime;
        float height;

        bool instanceStarted = false;

        Vector3 newPos = Vector3.zero;        

        CheckItemRemainCount();

        while (timer < shakeTime)
        {
            timer += Time.deltaTime;

            normalizedTime = timer * inverseShakeTime;
            height = Mathf.Lerp(0f, maxShakeHeight, Mathf.Sin(normalizedTime * Mathf.PI));
            
            newPos.y = height;

            spriteTransform.localPosition = newPos;

            if (normalizedTime > 0.5f && !instanceStarted)
            {
                instanceStarted = true;
                StartInstanceItem(spriteTransform.position, transform.position + Vector3.up);
            }

            yield return null;
        }

        spriteTransform.localPosition = Vector3.zero;
        CheckHitable();
        detectorWhenShaking.enabled = false;
        shakenObjects.Clear();
        yield break;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if(collision.TryGetComponent<IShakeable>(out IShakeable shakeable) &&
            !shakenObjects.Contains(shakeable))
        {
            shakenObjects.Add(shakeable);
            shakeable.Shake();
        }
    }
}
