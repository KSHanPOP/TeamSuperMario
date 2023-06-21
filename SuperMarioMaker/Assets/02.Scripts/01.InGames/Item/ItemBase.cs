using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected float instanceSequnceTime;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOder.Item;
    }
    protected virtual void StartAction()
    {
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public virtual void StartInstance(Vector2 destPos)
    {
        StartCoroutine(InstanceCoroutine(destPos));
    }

    public virtual IEnumerator InstanceCoroutine(Vector2 destPos)
    {
        float timer = 0f;
        float inverseInstanceTime = 1 / instanceSequnceTime;
        Vector3 startPos = transform.position;

        while (timer < instanceSequnceTime)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, destPos, timer * inverseInstanceTime);

            yield return null;
        }

        transform.position = destPos;
        StartAction();
        yield break;
    }

}
