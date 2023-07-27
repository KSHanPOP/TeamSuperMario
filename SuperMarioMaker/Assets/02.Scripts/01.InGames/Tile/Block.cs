using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField]
    private bool isTransparent;
    public bool IsTransparent { set { isTransparent = value; } }

    [SerializeField]
    protected Sprite spriteAfterUseItem;

    [SerializeField]
    protected EnumItems itemType;

    public EnumItems ItemType { set { itemType = value; } }

    [SerializeField]
    protected int itemCount;

    public int ItemCount { set { itemCount = value; } }

    [SerializeField]
    protected float shakeTime;

    [SerializeField]
    protected float maxShakeHeight;

    [SerializeField]
    protected float rayLength = 0.9f;
    protected Vector2 rayStartPos;

    protected bool isHitable = true;    

    protected SpriteRenderer spriteRenderer;

    protected Transform spriteTransform;

    protected ItemSpawnManagers spawnManagers;

    private HashSet<IShakeable> shakenObjects = new HashSet<IShakeable>();

    protected Transform dynamicObjHolder;

    private bool isCrahsed = false;

    private Coroutine shakeCoroutine;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Block;
        spriteTransform = spriteRenderer.transform;

        rayStartPos = (Vector2)transform.position + new Vector2(-rayLength * 0.5f, 0.55f);
    }
    protected virtual void Start()
    {
        spawnManagers = ItemSpawnManagers.Instance;
        dynamicObjHolder = TileManager.Instance.DynamicObjHolder;
    }

    public virtual void InitSetting()
    {
        //itemCount = itemCount > 0 ? itemCount : 1;

        if (itemCount == 0)
            itemType = EnumItems.Blank;

        if(itemCount > 0 && itemType == EnumItems.Blank)
            itemType = EnumItems.Coin;            
        
        SetStartSprite();
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

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }
    public virtual void BigHit()
    {
        if (!isHitable)
            return;

        if(itemType == EnumItems.Blank)
        {
            var instanced = Instantiate(ItemSpawnManagers.Instance.prefabs[(int)EnumItems.Blank], transform.position, Quaternion.identity, dynamicObjHolder);

            instanced.GetComponent<BlockCrashAnimation>().OnCrash();

            spriteRenderer.color = Color.clear;            
            GetComponent<Collider2D>().isTrigger = true;
            isCrahsed = true;
        }

        NormalHit();
    }

    protected virtual void StartInstanceItem(Vector2 startPos, Vector2 destPos)
    {
        if (itemType == EnumItems.Blank)
            return;

        var instanced = Instantiate(spawnManagers.prefabs[(int)itemType], startPos, Quaternion.identity, dynamicObjHolder);
        instanced.GetComponent<ItemBase>().StartInstance(destPos);
    }

    protected virtual void CheckItemRemainCount()
    {
        if (isCrahsed)
            return;

        switch (itemType)
        { 
            case EnumItems.Blank:
                SoundManager.Instance.PlaySFX("Bump");
                return;
            case EnumItems.Coin:
                SoundManager.Instance.PlaySFX("Coin");
                break;
            case EnumItems.Mushroom:
                SoundManager.Instance.PlaySFX("Item");
                break;
            case EnumItems.FireFlower:
                SoundManager.Instance.PlaySFX("Item");
                break;
        }

        --itemCount;

        if (itemCount <= 0)
            spriteRenderer.sprite = spriteAfterUseItem;
    }

    protected virtual void CheckHitable()
    {
        if(isCrahsed)
        {
            StopCoroutine(shakeCoroutine);
            Destroy(gameObject);
        }

        if (itemType == EnumItems.Blank)
        {
            isHitable = true;
            return;
        }

        isHitable = itemCount > 0;
    }

    protected virtual IEnumerator ShakeCoroutine()
    {
        isHitable = false;       

        float timer = 0f;
        float inverseShakeTime = 1 / shakeTime;        
        float normalizedTime;
        float height;

        bool instanceStarted = false;

        Vector3 newPos = Vector3.zero;

        int layerMask = LayerMask.GetMask("Invincible", "Monster", "Coin");

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

            ShakeColliders(layerMask);

            yield return null;
        }

        spriteTransform.localPosition = Vector3.zero;
        shakenObjects.Clear();
        CheckHitable();        
        yield break;
    }
    private void ShakeColliders(int layerMask)
    {
        var hits = Physics2D.RaycastAll(rayStartPos, Vector2.right, rayLength, layerMask);

        foreach(var hit in hits)
        {

            if(hit.transform.TryGetComponent(out IShakeable shakeable) &&
                !shakenObjects.Contains(shakeable))
            {
                if (hit.collider.CompareTag("Monster"))
                    ScoreManager.Instance.GetScore(200, hit.transform.position);

                shakeable.Shake(transform.position);
                shakenObjects.Add(shakeable);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(rayStartPos, rayStartPos + Vector2.right * rayLength);
    }
}
