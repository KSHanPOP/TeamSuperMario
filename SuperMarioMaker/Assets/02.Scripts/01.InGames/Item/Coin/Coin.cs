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
        if (!collision.CompareTag("Player"))
            return;

        SoundManager.Instance.PlaySFX("Coin");
        Logger.Debug("get score");
        Logger.Debug("get coin");

        Destroy(gameObject);
    }
}
