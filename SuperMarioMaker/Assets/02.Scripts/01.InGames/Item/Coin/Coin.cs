using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Item;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Logger.Debug("get score");
        Logger.Debug("get coin");

        Destroy(gameObject);
    }
}
