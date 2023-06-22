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

    //[SerializeField]
    //protected int coinCount;

    //public int CoinCount { set { coinCount = value; } }

    [SerializeField]
    protected int itemCount;

    public int ItemCount { set { itemCount = value; } }

    [SerializeField]
    protected float shakeTime;

    [SerializeField]
    protected float maxShakeHeight;

    protected bool isHitable = true;    

    protected SpriteRenderer spriteRenderer;

    protected Transform spriteTransform;

    protected ItemSpawnManagers spawnManagers;

    private HashSet<IShakeable> shakenObjects = new HashSet<IShakeable>();

    protected GameObject instanced;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOder.Block;
        spriteTransform = spriteRenderer.transform;        
    }
    protected virtual void Start()
    {
        spawnManagers = ItemSpawnManagers.Instance;        
    }

    public virtual void InitSetting()
    {
        itemCount = itemCount > 0 ? itemCount : 0;
        
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
        
        StartCoroutine(ShakeCoroutine());
    }
    public virtual void BigHit()
    {
        if (!isHitable)
            return;

        if(itemType == EnumItems.Blank)
        {
            instanced = Instantiate(ItemSpawnManagers.Instance.prefabs[(int)EnumItems.Blank], transform.position, Quaternion.identity);

            instanced.GetComponent<BlockCrashAnimation>().OnCrash();

            spriteRenderer.color = Color.clear;            
            Destroy(gameObject, 3f);
        }

        NormalHit();
    }

    protected virtual void StartInstanceItem(Vector2 startPos, Vector2 destPos)
    {
        if (itemType == EnumItems.Blank)
            return;

        instanced = Instantiate(spawnManagers.prefabs[(int)itemType], startPos, Quaternion.identity);
        instanced.GetComponent<ItemBase>().StartInstance(destPos);
    }

    protected virtual void CheckItemRemainCount()
    {
        if (itemType == EnumItems.Blank)
            return;

        --itemCount;

        if (itemCount == 0)
            spriteRenderer.sprite = spriteAfterUseItem;
    }
    protected virtual void CheckHitable()
    {
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

        int layerMask = LayerMask.GetMask("Invincible", "Monster");

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
        CheckHitable();        
        shakenObjects.Clear();
        yield break;
    }
    private void ShakeColliders(int layerMask)
    {
        var hits = Physics2D.RaycastAll((Vector2)transform.position + Vector2.one * 0.55f, Vector2.left, 1.1f, layerMask);

        foreach(var hit in hits)
        {
            if(hit.transform.TryGetComponent<IShakeable>(out IShakeable shakeable) &&
                !shakenObjects.Contains(shakeable))
            {
                shakeable.Shake(transform.position);
                shakenObjects.Add(shakeable);
            }
        }
    }

    protected void OnDestroy()
    {   
        if (instanced != null)
            Destroy(instanced);
    }
}
