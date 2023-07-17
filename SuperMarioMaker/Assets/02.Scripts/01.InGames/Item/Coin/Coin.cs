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

        if(SceneLoader.Instance.State == SceneState.Tool)
        {
            ToolManager.Instance.AddCoin(1);
            ToolManager.Instance.AddScore(100);
        }
        else
        {
            
        }

        Destroy(gameObject);
    }
}
