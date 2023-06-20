using System.Collections;
using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField]
    private bool isTransparent;

    [SerializeField]
    private Sprite spriteAfterUseItem;

    [SerializeField]
    private EnumItems itemType;

    [SerializeField]
    private int coinCount;

    private int itemCount;

    [SerializeField]
    private float shakeTime;

    [SerializeField]
    private float maxShakeHeight;

    private bool isHitable = true;
    
    private SpriteRenderer spriteRenderer;

    private Transform spriteTransform;

    private ItemSpawnManagers spawnManagers;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteTransform = spriteRenderer.transform;

        itemCount = itemType == EnumItems.Coin ? coinCount : 1;
        SetStartSprite();
    }
    private void Start()
    {
        spawnManagers = ItemSpawnManagers.Instance;
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

    public void StartInstanceItem(Vector2 startPos, Vector2 destPos)
    {
        if (itemType == EnumItems.None)
            return;        

        var item = Instantiate(spawnManagers.prefabs[(int)itemType], startPos, Quaternion.identity);
        item.GetComponent<ItemBase>().StartInstance(destPos);
    }

    private void CheckItem()
    {
        if (itemType == EnumItems.None)
            return;

        --itemCount;

        if (itemCount == 0)
            spriteRenderer.sprite = spriteAfterUseItem;
    }
    private void CheckHitable()
    {
        if (itemType == EnumItems.None)
        {
            isHitable = true;
            return;
        }

        isHitable = itemCount > 0;
    }

    IEnumerator ShakeCoroutine()
    {
        isHitable = false;

        float timer = 0f;
        float divShakeTime = 1 / shakeTime;        
        float nomalizedTime;
        float height;

        bool instanceStarted = false;

        Vector3 newPos = Vector3.zero;        

        CheckItem();

        while (timer < shakeTime)
        {
            timer += Time.deltaTime;

            nomalizedTime = timer * divShakeTime;
            height = Mathf.Lerp(0f, maxShakeHeight, Mathf.Sin(nomalizedTime * Mathf.PI));
            
            newPos.y = height;

            spriteTransform.localPosition = newPos;

            if (nomalizedTime > 0.5f && !instanceStarted)
            {
                instanceStarted = true;
                StartInstanceItem(spriteTransform.position, transform.position + Vector3.up);
            }

            yield return null;
        }

        spriteTransform.localPosition = Vector3.zero;
        CheckHitable();
        yield break;
    }
}
